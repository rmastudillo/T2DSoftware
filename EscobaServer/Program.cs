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
var escuchando = true;
var state = true;
TcpListener listener = new TcpListener(IPAddress.Any, 8000);
listener.Start();
while (true)
{
    if (escuchando)
    {
        Console.Write("Esperando por una conexión...\n");
        TcpClient tc = listener.AcceptTcpClient();
        Console.Write($"Cliente {num_players} Conectado!\n");
        ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { tc, num_players });
        num_players += 1;
        if (num_players == 3)
        {
            escuchando = false;
        }
    }
    if (!state)
    {
        listener.Stop(); // dejamos de escuchar
        break;
    }
}

void ThreadProc(object obj)
{
    object[] param = obj as object[];
    var client = (TcpClient)param[0];
    NetworkStream ns = client.GetStream();

    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    var mensaje = reader.ReadLine();
    while (mensaje != "Salir")
    {
        Console.WriteLine($"El cliente {param[1]} dice: " + mensaje);
        writer.WriteLine(mensaje);
        writer.Flush();
        mensaje = reader.ReadLine();
    }
    num_players -= 1;
    Console.Write("Un cliente abandonó el servidor :(\n");
    client.Close(); // cerramos la conexio ́n
    if (num_players == 1)
    {
        Console.Write(num_players + "\n");
        Console.Write("cerrando servidor\n");
        state = false;
    }
}
// if (mensaje == "Salir")
// {
//     num_players -= 1;
// }

// Console.WriteLine("El cliente dice: " + reader.ReadLine());
// writer.WriteLine("Dejame en paz! >=(");
// writer.Flush();
// tc.Close(); // cerramos la conexio ́n
// listener.Stop(); // dejamos de escuchar