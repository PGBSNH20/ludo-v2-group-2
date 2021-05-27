# Base /api
All of the controllers will start with /api/

Regarding the *Request type: Patch* I had to do some research as to how to do it since it was quite different controller type than the others, but since we update the information on the boarStates each time that a piece has been moved, so I went into the official documentation to try to find out how to do it and found this nice link with a step by step way to do it:  [Microsoft documentation for patch request type](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0)
but then I had issues because I wanted to patch it depending on the Id but the documentation didn't show that so I looked into this other website and found my solution there!
[Second reference for patching](https://www.roundthecode.com/dotnet/asp-net-core-web-api/asp-net-core-api-how-to-perform-partial-update-using-http-patch)

## Board Controllers
Request Type | URL | Description
------------ | --- | -----------
| Get | /boards | Gives back all of the Boards from the database. |
| Get | /boards/{id} | Depending on Id given it will give back the board. |
| Get | /boards/history | Gives back a list of games that have been finished. |
| Get | /boards/unfinished | Gives back a list of unfinished boards, this is for loading games. |
| Post | /boards | Creates a new board. |
| Delete | /boards/{id} | Deletes a board depending on the id given. Admin only |

## BoardStates
Request Type | URL | Description
------------ | --- | -----------
| Get | /boardStates | Gives back a list of all board states. |
| Get | /boardStates/{id} | Gives back a list of board states depending on Id given. |
| Get | /boardStates/{boardId} | Gives back a list of all board states for a board. |
| Get | /boarStates/{boardId}/{playerId} | Gives back a list of a players board state for that board. |
| Patch | /boardStates/{id} | Changes a board state partially depending on the Id given. Used for updating a players piece when it's moved. |
| Delete | /boardStates/{id} | Deletes a board state depending on the id given, used to delete pieces once they reach the goal. |

## Colors
Request Type | URL | Description
------------ | --- | -----------
| Get | /colors | Gives back a list of all the colors. |
| Get | /colors/{id} | Gives back a color depending on the Id given. |
| Post | /colors | Creates a new color |
| Patch | /colors/{id} | Depending on Id given, it will change the information in that color partially. Admin usage. |
| Delete | /colors/{id} | Deletes a color depending on Id given. |

## Game Controllers
Request Type | URL | Description
------------ | --- | -----------
| Get | /game/boards/{boardId}/players | You give it a board Id, and you get back a list of players for that board |
| Get | /game//players/{playerId} | Gives back a player, depending on the Id given. |
| Post | /game/players | Creates a new player. |
| Get | /game/boards/{boardId}/boardStates | Gives back a list of all boardStates of a game, according to the boardId. |
| Get | /game/boards/{boardId}/players/{playerId}/boardStates | According to the boardId and playerId given it will give back the pieces for that player. |
| Get | /game/boards/{boardId}/players/{playerId}/pieces/{pieceNumber}/boardState| Gets the piece for a specific player in a specific board |
| Get | /game/players/{playerId}/color | Gives back the color that the player with that Id has. |
| Get | /game/colors | Gives back a list of all colors |
| Get | /game/boards | Gives back a list of all boards |
| Get | /game/boards/{boardId} | Gives back a board information according to the board Id given. |
| Post | /game/new | Creates a new game, so 4 boardStates per player, and a board. |
| Post | /game/boards/{boardId}/skipround | Get a specific board, and gets the current player and skips on to the next player. |
| Get | /game/boards/{boardId}/players/{playerId}/next | Gives back the next player, According to the player given. 
| Get | /game/boards/{boardId}/players/{playerId}/haswon | Checks if that player, in a specific board has won, returns back a boolean |
| Get | /game/boards/{boardId}/GameOver | Checks if the game is over, if there is only one player left then the game will be over |
| Post | /game/boards/{boardId}/players/{playersId}/leaveBase | Finds a piece in the base for that player in that board, and will take it out, will return a boolean if its successfull or not. |
| Get | /game/boards/{boardId}/squares/{position}/isOcupied | Will return a boolean if a specific square in the board given is ocupied. |
| /game/boards/{boardId}/players/{playerId}/safezone/{position}/isOccupied | Gets a player from a specific board and checks if the position is currently occupied, returns a boolean |
| Get | /game/boards/{boardId}/players/{playerId}/startingPosition | Gives back the position for that player, since each player starts at different positions (t.ex: 0,13,26,39) |
| Post | /game/boards/{boardId}/players/{playerId}/pieces/{pieceNumber}/{steps} | Gets a specific player piece for a specific board and moves it if its a valid move, returns a boolean if it can move or not. |
| Get | /game/boards/{boardId}/players/{playerId}/pieces/base | Gives back a count of pieces that are in the base for that specific player in that specific board. |
| Get | /game/boards/{boardId}/players/{playerId}/goal | Gives back a count of pieces that are in the goal for that specific board. |
| Get | /game/boards/{boardId}/players/{playerId}/pieces/movable/{steps} | checks if there is any available moves for that palyer in that specific board, according to the steps given. |
| Get | /game/boards/{boardId}/players/{playerId}/pieces/pos/{piecePosition}/safezone/{isInSafeZone}/targetposition/{steps} | Gives you the target position, if it is going to go into the safe zone. Needed this one to make the highlighting positble for the posible moves. |
| Get | /game/boards{id}/winners | Gives back the winners, to be able to display in the winners page. |

Something to note... *Game Controllers* was created very last minute in order to give logic to controllers, Also something to note is that some of these controllers were made in other Controller files, but we left them like this so the unit tests would make sense. 

## Players Controller
Request Type | URL | Description
------------ | --- | -----------
| Get    | /players         | Gives back all of the Players from the database.                   |
| Get    | /players/{id}    | Gives back a player according to his id from the database.         |
| Put    | /players/{id}    | Updates a whole player details according to the given id.          |
| Post   | /players         | To create a player and save it to the database.                    |
| Patch  | /players/{id}    | To Update a specific data in a player and save it to the database. |
| Delete | /players/{id}    | To delete a player from the database according to the given id.    |

## Winners Controller
Request Type | URL | Description
------------ | --- | -----------
| Get    | /winners         | Gives back all of the winners from the database.                   |
| Get    | /winners/{id}    | Gives back a winner according to his id from the database.         |
| Get    | /boards/{id}     | Gives back a specific board according to the id given.             |
| Put    | /winners/{id}    | Updates a whole winner details according to the given id.          |
| Post   | /winners         | To create a winner and save it to the database.                    |
| Delete | /winners/{id}    | To delete a winner from the database according to the given id.    |
