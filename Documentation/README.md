## General Overview
We decided to use the Ludo Engine from the old project (from the database access course) some of the [database tables](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) were changed so it fits this project better.

In order for the program to run:
1. First the docker file has to be running in the background, so make sure that the docker compose is up.
2. Make sure that you add-migration, and update-database (if there is any issues make sure that the migrations folder is deleted and retry the update-database step)
3. RightClick the LudoApi solution and press "Build"
4. Right click the LudoApi solution again and click on "Open File in File Explorer" then, click "bin" then, "Debug" then "net5.0" and finally you want to click on the LudoApi.exe, this **must** be on in the background in order to execute the Api, so we can make calls to it.

![How to Docker compose, and execute LudoApi](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/DockerComposeAndLudoApiExecuteHowTo.gif)

## [Controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md)
We made different controllers according to the [tables](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) we have on the Ludo Engine, We need all the controllers in order to get/post/patch/delete information in the database.

## Docker-Compose
Made a docker file in order to be able to run the SQL database, the files were removed from the Visual Studio solution, since they are running in the background they don't need to be there. 

![image](https://user-images.githubusercontent.com/70092696/118378991-ad003480-b5d7-11eb-9a80-d76c1006e12d.png)

## Authentication and Authorization
The files got added in the LudoApi, along with in the start up, but due to time constraints it never got implemented onto the FrontEnd

References:

[Authenticate and Authorization with ASP.NET Core](https://www.c-sharpcorner.com/article/authentication-and-authorization-in-asp-net-core-web-api-with-json-web-tokens/)

## LudoFrontEnd
We also decided on using the same flow chart for the Razor Pages to guide our [Main Menu, New Game, Load, and history](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Ludo%20-%20Menu.jpg)

Along with [Flow chart for Ludo Game](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Ludo%20-%20Basic%20Game%20Flow.jpg) 

### LudoApi
In this folder we will be using RestSharp in order to make the api calls to use the controllers, this way the Frontend/website will be making Rest Api calls that control the logic.

### Razor Pages Home Page
This page contains the main menu of the game where there is several options for the  user to choose.

* New Game: Will redirect the user to another page were he can enter the number of players and the players details to start a new game.
* Load Game: Will redirect the user to another page were he can choose between the games which is not finished yet.
* Learn How To Play: Will redirect the user to another page were  there will be a video which explain briefly the game rules.
* Credits: Will show the credits :)
* Exit: Will return the user to the main page.

### Razor Pages New Game
In this page the user/s will enter the number of players and their details. By default it will always show 4 players input fields, but when the user changes the number of players to 2 or 3 then the input fields will change according to the number of players.
When the players are done from writing their names and colors
they will click on the button Add Players! then all the details will be added to a list and each of them will automatically roll a dice once, to see who will get the highest number in order to start first.
Then  we make some REST Api calls in order to save the players to the database and to create a new board.


### [Razor Page Play!](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Razor%20Pages%20Play!.md) 
This is the page where you actually play the game, it has it's own set of [Controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md) called GameControllers, this is were we make REST Api calls with logic in order to make moves and make sure they are getting checked by before they go into the database. This one was perticularly hard, and realized that the original plan that we had on just wasn't going to work very well..

**Original Plan** was to have a LudoApi, LudoEngine, and a LudoFrontEnd, and do the api calls once the logic was decided on the LudoFrontEnd... but it got quite complicated to write the code there, and then realized that they weren't really restfull Api calls... 
So after some thought it was decided that **New Plan** would be making new controllers would be made that would have logic in the Api calls, However the Dice will be in the LudoFrontEnd because we will not be saving the dice information onto the database.

I made a seperate file to explain more about it, because it is alot of different components that took to make this [Play! Page](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Razor%20Pages%20Play!.md)

## Game over!
Once a game has been completed, so we get all of the winners for that Board it will automatically take the user to the Game Over! page and display the winners.

## [LudoTests](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoTests.md)
Here will be our explanation of the testing we did, they are all Xunit tests.

## Things we regret/ could have done better:

**Alejandra's regrets**
