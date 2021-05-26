## General Overview
We decided to use the Ludo Engine from the old project (from the database access course) some of the [database tables](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) were changed so it fits this project better.

## [Controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md)
We made different controllers according to the [tables](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) we have on the Ludo Engine, We need all the controllers in order to get/post/patch/delete information in the database.

## Docker-Compose
Made a docker file in order to be able to run the SQL database, the files were removed from the Visual Studio solution, since they are running in the background they don't need to be there. 

![image](https://user-images.githubusercontent.com/70092696/118378991-ad003480-b5d7-11eb-9a80-d76c1006e12d.png)

## Razor Pages
We also decided on using the same flow chart for the Razor Pages to guide our [Main Menu, New Game, Load, and history](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Ludo%20-%20Menu.jpg)

Along with [Flow chart for Ludo Game](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Ludo%20-%20Basic%20Game%20Flow.jpg) 

### Razor Pages: Play!
This is the page where you actually play the game, it has it's own set of [Controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md) called GameControllers, this is were we make REST Api calls with logic in order to make moves and make sure they are getting checked by before they go into the database. This one was perticularly hard, and realized that the original plan that we had on just wasn't going to work very well..
**Original Plan:** was to have a LudoApi, LudoEngine, and a LudoFrontEnd, and do the api calls once the logic was decided on the LudoFrontEnd... but it got quite complicated to write the code there, and then realized that they weren't really restfull Api calls... 
So after some thought it was decided that **New Plan** would be making new controllers would be made that would have logic in the Api calls, However the Dice will be in the LudoFrontEnd because we will not be saving the dice information onto the database.

## [LudoTests](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoTests.md)
Here will be our explanation of the testing we did, they are all Xunit tests.

## Things we regret/ could have done better:
**Alejandra's regrets**
I picked to be incharge of the Play! LudoFrontEnd page, where is where you actually play a game, I had read the instructions wrong.. wasted 2 weeks, and realized when I started to implement actual board logic into the ludogame.js / LudoGame.cshtml/.cs that infact I needed to make the logic in the controllers, and use the controllers to stir and play the game,  and the .js script is there to make the api calls to make the board work in the razor pages, Thing that I am proud is that now the game could be played in Postman or in Swagger if someone wanted to. But because it was days before the actual turn in I had to cut corners: no unit testing for the GIGA file *GameController* also that Giga file could have definitely seperated into smaller-more-readable files...
