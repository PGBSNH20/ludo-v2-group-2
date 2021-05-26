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

## PlayersControllerTests.cs

## TestContext.cs
Here we have the context that we will be using for [BoardsControllerTests.cs](#anchor-boards)

## WinnersControllerTests.cs
