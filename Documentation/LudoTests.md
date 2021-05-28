## BoardsControllerTests.cs 
All of the tests here are Facts, we tests all of the [controllers](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md) for the boards file over in the LudoApiFolder. In the following test example we start out by creating our new TestContext, then I (Alejandra) decided that since alot of the tests have to have information, I decided to seed it, the seeding is in the TestContext.Cs, on this perticular one it is simple seeding the database with information for the [Board table](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) then we call the controller we need, in this case we are testing that we get 3 boards in that table, so we want the GetBoards(); then we save the information as a List<BoardDTO> boards, then we tested that the count is equal to 3, then that the board at the index of 1 the id is  equal to 2,  that the last time played for the first board is the same as what we expect, and finally that the last board is finished.

![image](https://user-images.githubusercontent.com/70092696/119712168-9de37700-be60-11eb-8c74-d867393deed0.png)

## BoardStatesControllerTets.cs
All of the tests here are also Facts, we tests the controllers for the [BoardStatesController](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/Crontrollers.md) file in the LudoApiFolder.
I wanted to explain Patching controller, it is a very different controller then the other types, it required more research, and also with testing it was harder to test because it doesn't do the same things as the other controllers..

![image](https://user-images.githubusercontent.com/70092696/119713646-37f7ef00-be62-11eb-9454-b4c42b36835d.png)

As the tests name tells, we are patching 1 BoardState, and we are expecting to get that string replaced
we start by telling it what context it will be using, then we seed that [Table](https://github.com/PGBSNH20/ludo-v2-group-2/blob/main/Documentation/ERDiagramLudo.png) with the appropriate information it needs, then we create the controller, and afterwards say which controller in that file we are using: PatchBoardSTate, we give it a 6, and a patchDoc.. the 6 is the BoardState Id, and the patchDoc is the  document in Json that we give it in order for the controller to do "its thing" we are telling the patch doc to replace the Pieceposition to 24. then we Test that the 24 is actually equal to the final patched form.

![image](https://user-images.githubusercontent.com/70092696/119714239-ed2aa700-be62-11eb-8d11-8e8117cad131.png)

Reference:
[Microsoft-Docs:JsonPatchDocument](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.jsonpatch.jsonpatchdocument-1?f1url=%3FappId%3DDev16IDEF1%26l%3DEN-US%26k%3Dk(JsonPatchDocument%601);k(DevLang-csharp)%26rd%3Dtrue&view=aspnetcore-5.0)


## ColorsControllerTests.cs
All tests are Facts, in this test example we will be showing how the Delete Controller works,

First we give it a new TestContext, then we seed the database with some colors, we then test that the color is actually there before deleting it, then we call the controller method to delete the color, we give it the color Id, and then we test that it is null.

![image](https://user-images.githubusercontent.com/70092696/119715480-5959da80-be64-11eb-88d7-dcf3d41b134b.png)

## PlayersControllerTests.cs
  
![image](https://user-images.githubusercontent.com/56867894/119989536-925e9000-bfc7-11eb-979e-47d342baf179.png)
  
In the PlayersControllerTests I used In Memory Database where The advantage of in-memory provider is that you can test the code of your application against an in-memory      database instead of installing and configuring the real database.
It is a general purpose database, designed strictly for the testing purpose of your application's code and not a relational database.
  
![image](https://user-images.githubusercontent.com/56867894/119992875-46154f00-bfcb-11eb-9866-cd1640695c31.png)
  
In this test we are making a Get request to the REST api in order to get back all the players saved in the database, the test is expecting a true value were it checks if the name " Anna" exist in the database. We first make the get request and we store all the players in result, then we loop through it to check if it contains any players, or if it contains a specific player name.
  
## TestContext.cs
Here we have the context that we will be using for BoardsControllerTests.cs I (Alejandra) really wanted to give testing a real go this project, first I make a class named this, TestContext. which inherits from LudoContext then in the configuring we call a Guid, that is simply creating a unique identifier, then we convert it into a string, so that we can have a "new" TestConext name each time and our tests can always come out correct because its a new database and we also use-in-momory-database.

![image](https://user-images.githubusercontent.com/70092696/119717513-adfe5500-be66-11eb-888b-b8584af246cb.png)

Then after is just the seeding that was needed for each unit test
References: 

[Microsoft-Docs: Guid Struct](https://docs.microsoft.com/en-us/dotnet/api/system.guid?view=net-5.0)

[StackOverflow: More than one DbContext was found](https://stackoverflow.com/questions/52311053/more-than-one-dbcontext-was-found)

[Miscrosoft-Docs: Unit Testing best practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)

[Microsoft-Docs: Testing code that uses EF core](https://docs.microsoft.com/en-us/ef/core/testing/)

[Microsoft-Docs: EF core testing sample](https://docs.microsoft.com/en-us/ef/core/testing/testing-sample)

## WinnersControllerTests.cs

![image](https://user-images.githubusercontent.com/56867894/119994342-cf795100-bfcc-11eb-8b67-f38f9f3da2c3.png)

In the WinnersControllerTests I used In Memory also and as shown we are making a Get request for a specific winner using its Id, so when we need to get the winner with Id 1 we are expecting back a winner object with an Id of 1.
So we store the winner object in result and we then check if the winner is you entered is equal the winner is we got back.
