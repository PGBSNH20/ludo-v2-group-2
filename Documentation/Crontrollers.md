# Base /api
All of the controllers will start with /api/

## Board Controllers
Request Type | URL | Description
------------ | --- | -----------
| Get | /Boards | Gives back all of the Boards from the database. |

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
