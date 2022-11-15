using EscobaServer;

var playerOne = new Player(0);
var playerTwo = new Player(1);
var game = new EscobaGame(playerOne,playerTwo);

game.Messages.MainMenu();
var serverMessage = Console.ReadLine();

switch (serverMessage)
{
    case "1":
        game.NewHand();
        break;
    case "2":
    {
        var server = new Server(game);
        server.StartListening();
        break;
    }
}