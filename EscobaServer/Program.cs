using EscobaServer;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

var Messages = new Messages();
var playerOne = new Player(0);
var playerTwo = new Player(1);
var game = new EscobaGame(playerOne, playerTwo);
game.NewHand();
game.Messages.MainMenu();
var serverMessage = Console.ReadLine();
if (serverMessage == "2")
{
    Console.WriteLine("Servidor corriendo!");
    var numPlayers = 1;
    var escuchando = true;
    var state = true;
    var listener = new TcpListener(IPAddress.Any, 8000);
    listener.Start();
    while (true)
    {
        if (escuchando)
        {
            Console.Write("Esperando por una conexión...\n");
            TcpClient tc = listener.AcceptTcpClient();
            Console.Write($"Cliente {numPlayers} Conectado!\n");
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { tc, numPlayers });
            numPlayers += 1;
            if (numPlayers == 3)
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
            var message = new List<string>(new[]
            {
                "########################################\n",
                "#   Bienvenido al juego de la escoba   #\n",
                "########################################\n"
            });
            var consolidateMessage = string.Join("", message);
            writer.WriteLine(consolidateMessage);
            writer.WriteLine(mensaje);
            writer.Flush();
            mensaje = reader.ReadLine();
        }
        numPlayers -= 1;
        Console.Write("Un cliente abandonó el servidor :(\n");
        client.Close(); // cerramos la conexión
        if (numPlayers == 1)
        {
            Console.Write(numPlayers + "\n");
            Console.Write("cerrando servidor\n");
            state = false;
        }
    }
}
