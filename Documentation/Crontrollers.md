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
| Get | /game/players/{playerId}/color | Gives back the color that the player with that Id has. |
| Get | /game/colors |
| Get | /game/boards |
| Get | /game/boards/{boardId} |
| Post | /game/new | Creates a new game, so 4 boardStates per player, and a board. |

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
