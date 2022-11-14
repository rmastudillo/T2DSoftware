using EscobaServer;
using System.Net;
using System.Net.Sockets;


var messages = new Messages();
var playerOne = new Player(0);
var playerTwo = new Player(1);
var game = new EscobaGame(playerOne,playerTwo);
//game.NewHand();
game.Messages.MainMenu();
var serverMessage = Console.ReadLine();
if (serverMessage == "2")
{
    var server = new Server(game);
    server.StartListening();
}
