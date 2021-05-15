# Base /api
All of the controllers will start with /api/

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

## Players Controller
Request Type | URL | Description
------------ | --- | -----------
| Get    | /players         | Gives back all of the Players from the database.                   |
| Get    | /players/{id}    | Gives back a player according to his id from the database.         |
| Put    | /players/{id}    | Updates a player details according to the given id.                |
| Post   | /players         | To create a player and save it to the database.                    |
| Delete | /players/{id}    | To delete a player from the database according to the given id.    |

## Winners Controller
Request Type | URL | Description
------------ | --- | -----------
| Get    | /winners         | Gives back all of the winners from the database.                   |
| Get    | /winners/{id}    | Gives back a winner according to his id from the database.         |
| Get    | /boards/{id}     | Gives back a specific board according to the id given.             |
| Put    | /winners/{id}    | Updates a winner details according to the given id.                |
| Post   | /winners         | To create a winner and save it to the database.                    |
| Delete | /winners/{id}    | To delete a winner from the database according to the given id.    |
