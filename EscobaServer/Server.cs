namespace EscobaServer;
using System.Net;
using System.Net.Sockets;
public class Server
{
    private int NumOfConnectedPlayers = 0;
    private bool Listening = true;
    private bool State = true;
    private TcpListener Listener = new(IPAddress.Any, 8000);

    public void SendMessage( StreamWriter writer,string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }
    private void ThreadProc(object obj)
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
                SendMessage(writer,consolidateMessage);
                mensaje = reader.ReadLine();
            }
            NumOfConnectedPlayers -= 1;
            Console.Write("Un cliente abandonó el servidor :(\n");
            client.Close(); // cerramos la conexio ́n
            if (NumOfConnectedPlayers == 0)
            {
                Console.Write(NumOfConnectedPlayers + "\n");
                Console.Write("cerrando servidor\n");
                State = false;
            }
        }

    public void StartListening()
    {
        Listener.Start();
        while (true)
        {
            if (Listening)
            {
                Console.Write("Esperando por una conexión...\n");
                TcpClient tc = Listener.AcceptTcpClient();
                Console.Write($"Cliente {NumOfConnectedPlayers + 1} Conectado!\n");
                ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { tc, NumOfConnectedPlayers });
                NumOfConnectedPlayers += 1;
                if (NumOfConnectedPlayers == 2)
                {
                    Listening = false;
                }
            }
            if (!State)
            {
                Listener.Stop();
                break;
            }
        }
    }

}
