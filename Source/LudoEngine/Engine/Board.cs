using LudoEngine.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Engine
{
    public class Board
    {
        private int _boardSize { get; set; }
        private int _safeZoneSize { get; set; }
        private int _maxPlayers { get; set; }
        public int BoardId { get; set; }
        private List<Player> Players { get; set; } = new();
        public Dictionary<int, Player> Winners { get; set; } = new();
        public List<Piece> BasePieces { get; set; } = new();
        public Dictionary<int, Piece> BoardPieces { get; set; } = new();
        // Top dictionary key: playerId, value: <position, piece>
        public Dictionary<int, Dictionary<int, Piece>> SafeZonePieces { get; set; } = new();

        // Individual positions for each player
        private Dictionary<int, int[]> PlayerPaths { get; set; } = new();
        public int[] PlayerOrder { get; set; }
        public Color[] Colors { get; set; }

        public Board() : this(52, 6, 4) { }

        public Board(int boardSize, int safeZoneSize, int maxPlayers)
        {
            _boardSize = boardSize;
            _safeZoneSize = safeZoneSize;
            _maxPlayers = maxPlayers;

            Colors = DbQuery.GetAllColors().Result.ToArray();
        }

        // Initializes the game.
        public async Task StartGame(int firstPlayerId, int boardId)
        {
            BoardId = boardId;
            SetPlayerOrder(firstPlayerId);
            await InitializeBoardStates(BasePieces);
        }

        private async Task InitializeBoardStates(List<Piece> basePieces)
        {
            List<DbBoardState> boardStates = basePieces.Select(basePiece => new DbBoardState
            {
                BoardId = BoardId,
                IsInBase = true,
                IsInSafeZone = false,
                PieceNumber = basePiece.PieceNumber,
                PiecePosition = 0,
                PlayerId = basePiece.PlayerId
            }).ToList();
            await DbQuery.InitializeBoardStates(boardStates);
        }

        public bool AddPlayer(Player player)
        {
            if (Players.Count >= _maxPlayers)
            {
                return false;
            }
            Players.Add(player);

            CreatePlayerPieces(player);
            CreatePlayerSafeZone(player);
            return true;
        }

        private void CreatePlayerSafeZone(Player player)
        {
            SafeZonePieces.Add(player.Id, new Dictionary<int, Piece>());
        }

        // Adds 4 base pieces for the player.
        private void CreatePlayerPieces(Player player)
        {
            for (int i = 0; i < 4; i++)
            {
                BasePieces.Add(new Piece { PlayerId = player.Id, ColorId = player.ColorId, PieceNumber = i + 1 });
            }
            CreatePlayerPaths(player);
        }

        private void CreatePlayerPaths(Player player)
        {
            int[] playerPath = new int[_boardSize];
            int startingPosition = GetStartingPosition(player);
            for (int i = 0; i < _boardSize; i++)
            {
                playerPath[i] = (i + startingPosition) % _boardSize;
            }

            PlayerPaths.Add(player.Id, playerPath);
        }

        private void SetPlayerOrder(int firstPlayerId)
        {
            PlayerOrder = new int[Players.Count];
            List<Player> orderedPlayers = Players.OrderBy(player => player.ColorId).ToList();
            int firstPlayerIndex = orderedPlayers.FindIndex(player => player.Id == firstPlayerId);
            for (int i = 0; i < orderedPlayers.Count; i++)
            {
                // using modulo, to have the offsetIndex be able to go from 3 to 0,
                // so if 1st player is 3rd on colorId, then it should use modulo to find color(which is our position of the playerOrder) 3 then 0, 1, 2, it should loop a
                // total of activePlayers (orderedPlayers.Count)
                int offsetIndex = (i + firstPlayerIndex) % orderedPlayers.Count;
                PlayerOrder[i] = orderedPlayers[offsetIndex].Id;
            }
        }

        // Returns if the piece was successfully moved.
        public async Task<bool> MovePiece(Piece piece, int steps)
        {
            //Player player = GetPlayer(piece);
            int targetPosition = GetTargetPosition(piece, steps);

            if (IsInSafeZone(piece) && IsValidSafeZoneMove(piece, targetPosition))
            {
                MovePieceInSafeZone(piece, targetPosition);
                return true;
            }

            if (IsMovingToSafeZone(piece, steps))
            {
                int remainingBoardSteps = GetRemainingBoardSteps(piece);
                int safeZonePosition = steps - remainingBoardSteps - 1;

                if (!IsValidSafeZoneMove(piece, safeZonePosition)) { return false; }

                MoveToSafeZone(piece, steps);

                return true;
            }

            if (IsValidBoardMove(piece, steps))
            {
                if (IsOccupied(targetPosition))
                {
                    BoardPieces.Remove(targetPosition, out Piece opponentPiece);
                    await DbQuery.UpdateDbState(BoardId, opponentPiece.PlayerId, opponentPiece.PieceNumber, 0, true, false);
                    BasePieces.Add(opponentPiece);
                }

                MovePieceOnBoard(piece, targetPosition);
                return true;
            }

            return false;
        }

        private bool CheckForDuplicatesInSafeZone(Piece piece)
        {
            Dictionary<int, Piece> playerSafeZone = GetSafeZonePieces(piece);
            List<Piece> safeZonePieces = playerSafeZone.Select(piecePair => piecePair.Value).ToList();

            Dictionary<int, int> result2 = safeZonePieces.GroupBy(x => x.PieceNumber)
               .ToDictionary(y => y.Key, y => y.Count());
            if (result2.Any(r => r.Value > 1))
            {
                return true;
            }
            return false;
        }

        public int AddWinner(Player player)
        {
            int placement = 1;
            if (Winners.Keys.Count > 0)
            {
                placement += Winners.Keys.Max();
            }
            Winners.Add(placement, player);
            return placement;
        }

        private Player GetPlayer(Piece piece)
        {
            return Players.First(player => player.Id == piece.PlayerId);
        }

        private bool IsValidSafeZoneMove(Piece piece, int targetPosition)
        {
            return targetPosition < _safeZoneSize && !IsOccupiedInSafeZone(piece, targetPosition);
        }

        private void MovePieceInSafeZone(Piece piece, int targetPosition)
        {
            if (targetPosition >= _safeZoneSize)
            {
                throw new Exception("Can't move piece beyond safe zone.");
            }
            int startPosition = GetPiecePosition(piece);
            Dictionary<int, Piece> playerSafeZone = GetSafeZonePieces(piece);
            playerSafeZone.Remove(startPosition);
            playerSafeZone.Add(targetPosition, piece);
        }

        public bool IsInSafeZone(Piece piece)
        {
            Dictionary<int, Piece> playerSafeZonePieces = GetSafeZonePieces(piece);
            return playerSafeZonePieces.Any(piecePair => piecePair.Value == piece);
        }

        private Dictionary<int, Piece> GetPlayerSafeZone(Player player)
        {
            return SafeZonePieces[player.Id];
        }

        private void MovePieceOnBoard(Piece piece, int targetPosition)
        {
            int startPosition = GetPiecePosition(piece);
            BoardPieces.Remove(startPosition);
            BoardPieces.Add(targetPosition, piece);
        }

        private void MoveToSafeZone(Piece piece, int steps)
        {
            int startPosition = GetPiecePosition(piece);
            int remainingBoardSteps = GetRemainingBoardSteps(piece);
            int safeZonePosition = steps - remainingBoardSteps - 1;
            Dictionary<int, Piece> playerSafeZone = GetSafeZonePieces(piece);

            BoardPieces.Remove(startPosition);
            playerSafeZone.Add(safeZonePosition, piece);
        }

        private int GetTargetPosition(Piece piece, int steps)
        {
            int startPosition = GetPiecePosition(piece);

            if (IsInSafeZone(piece))
            {
                return startPosition + steps;
            }

            if (IsMovingToSafeZone(piece, steps))
            {
                int remainingBoardSteps = GetRemainingBoardSteps(piece);
                return steps - remainingBoardSteps - 1;
            }

            return GetNewPathPosition(piece, steps);
        }

        private int GetNewPathPosition(Piece piece, int steps)
        {
            int playerId = piece.PlayerId;
            int[] playerPath = PlayerPaths[playerId];
            int startingPosition = GetPiecePosition(piece);
            int startPositionIndex = Array.IndexOf(playerPath, startingPosition);
            return playerPath[startPositionIndex + steps];
        }

        private bool IsLoopingAroundBoard(Piece piece, int steps)
        {
            int playerId = piece.PlayerId;
            int[] playerPath = PlayerPaths[playerId];
            int startingPosition = GetPiecePosition(piece);
            int startPositionIndex = Array.IndexOf(playerPath, startingPosition);

            return startPositionIndex + steps >= playerPath.Length;
        }

        private bool IsMovingToSafeZone(Piece piece, int steps)
        {
            return !IsInSafeZone(piece) && IsLoopingAroundBoard(piece, steps);
        }

        private int GetRemainingBoardSteps(Piece piece)
        {
            int playerId = piece.PlayerId;
            int[] playerPath = PlayerPaths[playerId];

            int lastIndex = playerPath.Length - 1;
            int piecePosition = GetPiecePosition(piece);
            int pieceIndex = Array.IndexOf(playerPath, piecePosition);

            return lastIndex - pieceIndex;
        }

        private bool IsValidBoardMove(Piece piece, int steps)
        {
            int targetPosition = GetTargetPosition(piece, steps);

            if (!BoardPieces.ContainsValue(piece)) { return false; }
            if (IsLoopingAroundBoard(piece, steps)) { return false; }
            if (IsOwnedBySamePlayer(piece, targetPosition)) { return false; }

            return true;
        }

        public bool IsAtGoal(Piece piece)
        {
            if (IsInSafeZone(piece))
            {
                int position = GetPiecePosition(piece);
                if (position >= _safeZoneSize - 1)
                {
                    return true;
                }
            }
            return false;
        }

        public void RemovePieceWhenAtGoal(Piece piece)
        {
            Dictionary<int, Piece> playerSafeZone = GetSafeZonePieces(piece);
            int position = GetPiecePosition(piece);
            bool isInSafeZone = IsInSafeZone(piece);
            if (isInSafeZone && position >= _safeZoneSize - 1)
            {
                if (!playerSafeZone.Remove(position))
                {
                    throw new Exception("Unable to remove piece from safe zone.");
                }
            }
        }

        private bool IsOwnedBySamePlayer(Piece piece, int position)
        {
            if (IsOccupied(position))
            {
                Piece targetPiece = GetPieceAt(position);
                if (targetPiece.ColorId == piece.ColorId)
                {
                    return true;
                }
            }

            return false;
        }

        private Piece GetPieceAt(int position)
        {
            Piece piece = BoardPieces[position];
            return piece;
        }

        public int GetPiecePosition(Piece piece)
        {
            if (IsInSafeZone(piece))
            {
                var playerSafeZone = GetSafeZonePieces(piece);
                return playerSafeZone.First(piecePair => piecePair.Value == piece).Key;
            }
            try
            {
                return BoardPieces.First(piecePair => piecePair.Value == piece).Key;
            }
            catch (Exception)
            {
                throw new Exception("Piece does not exist.");
            }
        }

        public bool IsGameOver()
        {
            return Players.Count - 1 == Winners.Count;
        }

        // Checks if BasePieces, BoardPieces and the SafeZone contains any pieces of the player.
        public bool HasPlayerWon(Player player)
        {
            Dictionary<int, Piece> playerSafeZone = GetPlayerSafeZone(player);

            int pieces = BasePieces.Count(piece => piece.PlayerId == player.Id);
            pieces += BoardPieces.Count(piecePair => piecePair.Value.PlayerId == player.Id);
            pieces += playerSafeZone.Count;
            return pieces == 0;
        }

        public bool LeaveBase(Player player)
        {
            int startPosition = GetStartingPosition(player);
            if (!IsOccupied(startPosition))
            {
                Piece firstBasePiece = BasePieces.FirstOrDefault(piece => piece.PlayerId == player.Id);
                if (firstBasePiece == null) { return false; }

                try
                {
                    BasePieces.Remove(firstBasePiece);
                    BoardPieces.Add(startPosition, firstBasePiece);
                    return true;
                }
                catch (Exception)
                {
                    throw new Exception("No more base pieces.");
                }
            }
            return false;
        }

        private bool IsOccupied(int position)
        {
            return BoardPieces.ContainsKey(position);
        }

        // Checks if the safeZone position is currently being occupied
        private bool IsOccupiedInSafeZone(Piece piece, int targetPosition)
        {
            int playerId = piece.PlayerId;
            Dictionary<int, Piece> playerSafeZone = SafeZonePieces[playerId];

            return playerSafeZone.ContainsKey(targetPosition);
        }

        public int GetStartingPosition(Player player)
        {
            Color[] sortedColors = Colors.OrderBy(color => color.Id).ToArray();
            Color playerColor;

            try
            {
                playerColor = sortedColors.First(color => color.Id == player.ColorId);
            }
            catch (Exception)
            {
                throw new Exception("Can't find the player's color Id.");
            }

            int playerPositionIndex = Array.IndexOf(sortedColors, playerColor);
            int playerPositionOffset = GetPlayerPositionOffset();
            return playerPositionIndex * playerPositionOffset;
        }

        private int GetPlayerPositionOffset()
        {
            return _boardSize / _maxPlayers;
        }

        public Player GetNextPlayer(Player player)
        {
            if (IsGameOver())
            {
                throw new Exception("Game is over, there are no more players.");
            }

            int nextPlayerIndex = (Array.IndexOf(PlayerOrder, player.Id) + 1) % PlayerOrder.Length;
            int nextPlayerId = PlayerOrder[nextPlayerIndex];
            Player nextPlayer = Players.First(player => player.Id == nextPlayerId);

            return HasPlayerWon(nextPlayer) ? GetNextPlayer(nextPlayer) : nextPlayer;
        }

        public List<Player> GetPlayers()
        {
            return new List<Player>(Players);
        }

        public bool HasActivePieces(Player player)
        {
            bool hasBoardPieces = BoardPieces.Any(piecePair => piecePair.Value.PlayerId == player.Id);

            Dictionary<int, Piece> playerSafeZone = GetPlayerSafeZone(player);
            bool hasSafeZonePieces = playerSafeZone.Any();

            return hasBoardPieces || hasSafeZonePieces;
        }

        public List<Piece> GetActivePieces(Player player)
        {
            List<Piece> pieces = GetBoardPieces(player)
                .Where(piecePair => piecePair.Value.PlayerId == player.Id)
                .Select(piecePair => piecePair.Value)
                .ToList();

            Dictionary<int, Piece> playerSafeZone = GetPlayerSafeZone(player);
            List<Piece> safeZonePieces = playerSafeZone.Select(piecePair => piecePair.Value).ToList();

            pieces.AddRange(safeZonePieces);

            return pieces;
        }

        public List<Piece> GetMovablePieces(Player player, int steps)
        {
            List<Piece> pieces = GetActivePieces(player);
            return pieces.Where(piece => IsMovable(piece, steps)).ToList();
        }

        public bool IsMovable(Piece piece, int steps)
        {
            int targetPosition = GetTargetPosition(piece, steps);

            if (IsInSafeZone(piece) && IsValidSafeZoneMove(piece, targetPosition)) { return true; }

            if (IsMovingToSafeZone(piece, steps))
            {
                int remainingBoardSteps = GetRemainingBoardSteps(piece);
                int safeZonePosition = steps - remainingBoardSteps - 1;

                return IsValidSafeZoneMove(piece, safeZonePosition);
            }

            return IsValidBoardMove(piece, steps);
        }

        public Dictionary<int, Piece> GetBoardPieces(Player player)
        {
            Dictionary<int, Piece> pieces = BoardPieces
                .Where(piecePair => piecePair.Value.PlayerId == player.Id)
                .ToDictionary(piecePair => piecePair.Key, piecePair => piecePair.Value);

            return pieces;
        }

        private Dictionary<int, Piece> GetSafeZonePieces(Piece piece)
        {
            return SafeZonePieces[piece.PlayerId];
        }

        public List<Piece> GetBasePieces(Player player)
        {
            return BasePieces.Where(piece => piece.PlayerId == player.Id).ToList();
        }

        public List<Piece> GetBasePieces()
        {
            return BasePieces;
        }

        public Dictionary<int, Piece> GetBoardPieces()
        {
            return BoardPieces;
        }
    }
}
