This page is for the actual game, to play an active game that is!

## CSS: LudoGame.css
This is to style the look of the board, I mostly stuck to vmin, I thought by default it makes it responsive. Vmin looks at height and width and takes the percentage of whichever of the two that is smallest.  
Some things worthy of mention:
* Used a pulsating effect to show the user which pieces they can move, and where they would move to.
* When its the players turn their Base zone they will get a Red border alerting them its their turn, and if they are playing in different browsers/computers the whole board will have a red border telling them its their turn. 

References:

[Vmin css-tricks.com](https://css-tricks.com/simple-little-use-case-vmin/)

[Pulsating effect]()

## Api Calls

## LudoGame.js
This file is for the webside so everything that is happening in the browser, so everything that doesn't need to be run in the server side it gets run here, for example the rolls, the animations like highlighting the pieces if they can move, the logic that tells the board its the next players so there needs to be a red-border, and it also lets us click when it's possible, this is all happening on the page.

## LudoGame.cshtml

## LudoGame.cshtml.cs
This file is used for the server side, 
* This is where we give our board coordinates. 
* Calls the logic for the board stuff, so calls methods/Api for the board to do stuff, here are some examples of the types of things you would find there:
 * Checks if the game is over
 * Loads a game
 * If a spot is occupied
