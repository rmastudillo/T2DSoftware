using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EscobaServer;

Console.WriteLine("Hello, World!");

var playerOne = new Player(0);
var playerTwo = new Player(1);
// var game = new EscobaGame(playerOne,playerTwo);
// game.NewHand();
TcpListener listener = new TcpListener(IPAddress.Any, 8000);
listener.Start();
TcpClient tc = listener.AcceptTcpClient();

NetworkStream ns = tc.GetStream();
StreamReader reader = new StreamReader(ns);
StreamWriter writer = new StreamWriter(ns);
Console.WriteLine("El cliente dice: " + reader.ReadLine());
writer.WriteLine("D ́ejame en paz! >=(");
writer.Flush();
tc.Close(); // cerramos la conexio ́n
listener.Stop(); // dejamos de escuchar