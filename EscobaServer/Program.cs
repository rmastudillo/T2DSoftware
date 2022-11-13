using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EscobaServer;

// https://stackoverflow.com/questions/5339782/how-do-i-get-tcplistener-to-accept-multiple-connections-and-work-with-each-one-i

Console.WriteLine("Servidor corriendo!");

var playerOne = new Player(0);
var playerTwo = new Player(1);
// var game = new EscobaGame(playerOne,playerTwo);
// game.NewHand();
int num_players = 1;
TcpListener listener = new TcpListener(IPAddress.Any, 8000);
listener.Start();
while (true)
{
    if (num_players <= 2)
    {
        Console.Write("Esperando por una conexión...\n");
    }
    TcpClient tc = listener.AcceptTcpClient();
    Console.Write($"Cliente {num_players} Conectado!\n");
    ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { tc, num_players });
    num_players += 1;
}

static void ThreadProc(object obj)
{
    object[] param = obj as object[];
    var client = (TcpClient)param[0];
    NetworkStream ns = client.GetStream();

    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    var mensaje = reader.ReadLine();
    Console.WriteLine($"El cliente {param[1]} dice: " + mensaje);
    writer.WriteLine(mensaje);
    writer.Flush();
    // Do your work here
}

// Console.WriteLine("El cliente dice: " + reader.ReadLine());
// writer.WriteLine("Dejame en paz! >=(");
// writer.Flush();
// tc.Close(); // cerramos la conexio ́n
// listener.Stop(); // dejamos de escuchar