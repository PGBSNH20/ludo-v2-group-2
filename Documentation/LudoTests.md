## BoardsControllerTests.cs 
All of the tests here are Facts, we tests all of the [controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md) for the boards file over in the LudoApiFolder. In the following test example we start out by creating our new TestContext, then I (Alejandra) decided that since alot of the tests have to have information, I decided to seed it, the seeding is in the TestContext.Cs, on this perticular one it is simple seeding the database with information for the [Board table](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) then we call the controller we need, in this case we are testing that we get 3 boards in that table, so we want the GetBoards(); then we save the information as a List<BoardDTO> boards, then we tested that the count is equal to 3, then that the board at the index of 1 the id is  equal to 2,  that the last time played for the first board is the same as what we expect, and finally that the last board is finished.

![image](https://user-images.githubusercontent.com/70092696/119712168-9de37700-be60-11eb-8c74-d867393deed0.png)






## BoardStatesControllerTets.cs
## ColorsControllerTests.cs
## PlayersControllerTests.cs
## TestContext.cs
Here we have the context that we will be using for [BoardsControllerTests.cs](#anchor-boards)
## WinnersControllerTests.cs
