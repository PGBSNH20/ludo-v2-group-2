This page is for the actual game, to play an active game that is!

## CSS: LudoGame.css
This is to style the look of the board, I mostly stuck to vmin, I thought by default it makes it responsive. Vmin looks at height and width and takes the percentage of whichever of the two that is smallest.  
Some things worthy of mention:
* Used a pulsating effect to show the user which pieces they can move, and where they would move to.

![Possible moves showing the pulse effect](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/PlayerRollsShowAllPossibleMoves.gif)

* Something to note is that the player pieces don't pulsate if you can't move them

![Example of not a possible move](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/MouseoverOfPossibleMoveAndCantPulsateWhenNotMovable.gif)

* When its the players turn their Base zone they will get a Red border alerting them its their turn, and if they are playing in different browsers/computers the whole board will have a red border telling them its their turn. 

![Who's turn is it?](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/WhosTurnIsIt.gif)

References:

[Vmin css-tricks.com](https://css-tricks.com/simple-little-use-case-vmin/)

[Pulsating effect](https://www.florin-pop.com/blog/2019/03/css-pulse-effect/)

[Loading Custom Css Files in Razor Pages](https://dev.to/amjadmh73/loading-custom-css-files-in-razor-pages-4no9)

## Api Calls
We use the 'Giga'-file called: **GameController.cs** this file holds all the logic for the controllers/api calls in order for the board to get information out of the database
Sadly the file is very messy due to lack of time.

## LudoGame.js
This file is for the webside so everything that is happening in the browser, so everything that doesn't need to be run in the server side it gets run here, for example the rolls, the animations like highlighting the pieces if they can move, the logic that tells the board its the next players so there needs to be a red-border, and it also lets us click when it's possible, this is all happening on the page.

* Added functionality that the player gets skipped if there is no possible moves for that person with that roll

![No possible move](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/NoPossibleMovePlayerGetsSkipped.gif)

* Punting an opponents piece is possible!

![Example of punting an opponent player's piece](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/PuntingOfAPiece.gif)

* If certain players have won the board will keep checking if its a Game over, if not then the current winners will not be able to play but it will display which place they have gotten

[Game is still going but we have some winners!](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/WhenPlayersHaveWon.png)

## LudoGame.cshtml
This is where the HTML stuff is 

## LudoGame.cshtml.cs
This file is used for the server side, this is the logic the LudoGame.cshtml.
* This is where we give our board coordinates. 
* Calls the logic for the board stuff, so calls methods/Api for the board to do stuff, here are some examples of the types of things you would find there:
  * Checks if the game is over
  * Loads a game
  * If a spot is occupied

## SignalR
This is where SignalR got implemented, if you have different browsers open it should automatically show when people have moved and rolled, when its *Your* turn in your browser the whole board will have a border to try to notify you its your turn, and you can see if anyone is cheating by refreshing the page to re-roll again! 😉

This is an example of how signalR works with the Ludo game board:

![SignalR example](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/LudoGifs/WhosTurnIsIt.gif)
