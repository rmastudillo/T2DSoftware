using EscobaServer;

Console.WriteLine("Hello, World!");

var playerOne = new Player(0);
var playerTwo = new Player(1);
var game = new EscobaGame(playerOne,playerTwo);
game.NewHand();