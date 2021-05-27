window.onload = highLightActivePlayer()

var roll = 0;
var movablePieces = [];

class Position {
    constructor(position, isInSafeZone) {
        this.position = position;
        this.isInSafeZone = isInSafeZone;
    }
}

async function playTurn() {
    if (window.roll == 0) {
        RollDie();
        window.movablePieces = await getMovablePieces();
        await skipRound();
        highlightMovablePieces();
        await highlightPossibleTargetSquares();
        addMoveClickListener();
        addMouseOverHighlight();
        await addLeaveBaseClick();
    }
}

async function skipRound() {
    if (window.movablePieces.length == 0 &&!(await canLeaveBase())) {
        console.log("nani");
        await apiSkipRound();
        NextTurn();
    }
}

async function addLeaveBaseClick() {
    removeLeaveBaseClicks();
    setTimeout(await (async function() {
        if (await canLeaveBase()) {
            let base = document.getElementById("ludoBase" + (activePlayerIndex + 1));
            base.classList.add("leaveBase");
            base.addEventListener("click", function() { leaveBase() }, false);
        }
    }), 5);
}

function removeLeaveBaseClicks() {
    let ludoBases = document.getElementsByClassName("ludoBase");
    for (const base of ludoBases) {
        base.classList.remove("leaveBase");
        base.removeEventListener("click", function() { leaveBase() }, false);
    }
}

async function canLeaveBase() {
    if (window.roll != 6) {
        return false;
    }
    if (await isOccupied(await apiGetStartPosition())) {
        return false;
    }
    if (await getBasePieceCount() <= 0) {
        return false;
    }
    return true;
}

function addMouseOverHighlight() {
    for (const piece of window.movablePieces) {
        let square = getSquare(activePlayerIndex, piece);
        let squarePiece = getSquarePiece(square);
        squarePiece.addEventListener("mouseover", highlightCurrentTargetOnly, false);
        squarePiece.addEventListener("mouseleave", highlightPossibleTargetSquares, false);
    }
}

function highlightSquare(id) {
    let square = document.getElementById(id);
    square.classList.add("targetSquare");
}

async function highlightCurrentTargetOnly(event) {
    removeAllSquareHighlights();
    let mouseoverSquare = event.relatedTarget.id;
    let isInSafeZone = mouseoverSquare.includes("p_");
    let positionIndex = mouseoverSquare.indexOf("sq_") + 3;
    let position = mouseoverSquare.substring(positionIndex);
    let piece = window.movablePieces.find(piece => piece.position == position && piece.isInSafeZone == isInSafeZone);

    let targetPosition = await fetchTargetPosition(window.boardId, window.activePlayerId, piece.position, piece.isInSafeZone, window.roll);
    let targetSquare = getSquare(window.activePlayerIndex, targetPosition);
    targetSquare.classList.add("targetSquare");
}

function highlightMovablePieces() {
    for (const piece of window.movablePieces) {
        let square = getSquare(activePlayerIndex, piece);
        let squarePiece = getSquarePiece(square);
        squarePiece.classList.add("movablePiece");
    }
}

function addMoveClickListener() {
    for (const piece of window.movablePieces) {
        let square = getSquare(activePlayerIndex, piece);
        let squarePiece = getSquarePiece(square);
        squarePiece.addEventListener("click", function() { apiMovePiece(piece) }, false);
    }
}

function getSquare(playerIndex, position) {
    let square;
    if (position.isInSafeZone) {
        if (position.position == 5) {
            square = document.getElementById("ludoGoal" + (playerIndex + 1));
        } else {
            square = document.getElementById("p_" + (playerIndex + 1) + "_sq_" + position.position);
        }
    }
    else {
        square = document.getElementById("sq_" + position.position);
    }
    return square;
}

function getSquarePiece(square) {
    return square.getElementsByClassName("piece")[0];
}

async function highlightPossibleTargetSquares() {
    removeAllSquareHighlights();
    setTimeout(async function() {
        for (const piece of window.movablePieces) {
            let targetPosition = await fetchTargetPosition(window.boardId, window.activePlayerId, piece.position, piece.isInSafeZone, window.roll);
            let targetSquare = getSquare(window.activePlayerIndex, targetPosition);
            targetSquare.classList.add("targetSquare");
        }
    }, 5);
}

function removeAllSquareHighlights() {
    let squares = document.getElementsByClassName("ludoSquare");
    for (const square of squares) {
        square.classList.remove("targetSquare");
    }
}

function highLightActivePlayer() {
    let elementId = "ludoBase" + (window.activePlayerIndex + 1);
    document.getElementById(elementId).style.borderColor = "red";
    document.getElementById(elementId).style.borderWidth = ".5vmin";
}

async function getMovablePieces() {
    let pieces = await fetchMovablePieces();
    var movablePieces = [];
    pieces.forEach(piece => {
        movablePieces.push(new Position(piece["piecePosition"], piece["isInSafeZone"]));
    });
    return movablePieces;
}

async function getBasePieceCount() {
    const result = await apiFetch('boards/' + window.boardId + '/players/' + window.activePlayerId + '/pieces/base/', 'GET');
    return result.length;
}

async function isOccupied(position) {
    const result = await apiFetch('boards/' + window.boardId + '/squares/' + position + '/isOccupied/', 'GET');
    return result;
}

//async function isOccupiedInSafeZone(position) {
//    const result = await apiFetch('boards/' + window.boardId + '/players/' + window.activePlayerId + '/safezone/' + position + '/isOccupied/', 'GET');
//    return result;
//}

async function leaveBase() {
    const url = 'boards/' + window.boardId + '/players/' + window.activePlayerId + '/leaveBase';
    const result = await apiFetch(url, 'POST');
    NextTurn();
}

async function apiMovePiece(position) {
    let pieceNumber = await getPieceNumberByPosition(position)
    const url = 'boards/' + window.boardId + '/players/' + window.activePlayerId + '/pieces/' + pieceNumber + '/' + window.roll;
    const result = await apiFetch(url, 'POST');
    NextTurn();
}

async function getPieceNumberByPosition(position) {
    const result = await apiFetch('boards/' + window.boardId + '/boardStates', 'GET');
    
    let pieceNumber = -1;
    result.forEach(state => {
        if(state["piecePosition"] == position.position && state["isInSafeZone"] == position.isInSafeZone && state["isInBase"] == false) {
            pieceNumber = state["pieceNumber"];
            return false;
        }
    });
        
    return pieceNumber;
}

async function apiSkipRound() {
    const result = await apiFetch('boards/' + window.boardId+ '/skipround', 'POST');
    return result;
}

async function fetchTargetPosition(boardId, playerId, piecePosition, isInSafeZone, steps)
{
   const url = 'boards/' + boardId + '/players/' + playerId + '/pieces/pos/' + piecePosition + '/safezone/' + isInSafeZone + '/targetposition/' + steps;
   const result = await apiFetch(url, 'GET');
   return new Position(result["position"], result["isInSafeZone"]);
}

async function fetchMovablePieces() {
    const url = 'boards/' + window.boardId+ '/players/' + window.activePlayerId + '/pieces/movable/' + window.roll;
    const result = await apiFetch(url, 'GET');
    return result;
}

async function apiGetStartPosition() {
    const url = 'boards/' + window.boardId+ '/players/' + window.activePlayerId + '/startingPosition';
    const result = await apiFetch(url, 'GET')
    return result;
}

async function apiFetch(url, verb) {
    verb = verb.toUpperCase();
    url = 'https://localhost:5001/api/game/' + url;
    const result = await fetch(url,
    {
        method: verb,
        headers: {
            "apikey": "Mountain24"
        }
    })
    .then(response => response.json())
    .then(data => {
        return data
    })
    .catch(error => {
        console.error(error);
    });
    return result;
}

function NextTurn() {
    InvokeSignalRMessage("reload", "")
}

function RollDie() {
    window.roll = Math.floor(Math.random() * 6) + 1;
    InvokeSignalRMessage("showroll", "" + window.roll)
}

async function ShowRoll(number) {
    let rollButton = document.getElementById("roll-button");
    rollButton.innerHTML = number;
    rollButton.removeEventListener("click", playTurn);
    rollButton.style.fontSize = "3vmin";
    rollButton.style.fontFamily = "sans-serif";
    rollButton.style.color = "white";
    rollButton.style.fontWeight = 700;
}

function InvokeSignalRMessage(action, data) {
    connection.invoke("SendMessage", action, data).catch(function (err) {
        return console.error(err.toString());
    }); 
}

document.getElementById("roll-button").addEventListener("click", playTurn);

var connection = new signalR.HubConnectionBuilder().withUrl("/ludoHub").build();

connection.on("ReceiveMessage", function (action, data) {
    if (action === "reload") {
        location.reload();
    }
    if (action === "showroll") {
        ShowRoll(data);
    }
});

connection.start();