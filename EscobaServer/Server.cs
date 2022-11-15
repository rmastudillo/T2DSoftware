using System.Threading.Tasks.Dataflow;

namespace EscobaServer;
using System.Net;
using System.Net.Sockets;

public class Client
{
    public StreamReader ClientReader { get; }
    public  StreamWriter ClientWriter { get; }
    public Client(StreamReader clientReader, StreamWriter clientWriter)
    {
        ClientReader = clientReader;
        ClientWriter = clientWriter;
    }
}
public class Server
{
    private int NumOfConnectedPlayers = 0;
    private bool Listening = true;
    private bool State = true;
    private TcpListener Listener = new(IPAddress.Any, 8000);
    public  Client FirstClient { get; set; }
    public  Client SecondClient { get; set; }
    private EscobaGame Game { get; }
    public void SendMessage( StreamWriter writer,string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }

    public Server(EscobaGame game)
    {
        Game = game;
    }
    private void SetClient(string? clientNumber, StreamReader clientReader, StreamWriter clientWriter)
    {
            switch (clientNumber)
            {
                case "0":
                    FirstClient = new Client(clientReader, clientWriter);
                    FirstClient.ClientWriter.Flush();
                    break;
                case "1":
                    SecondClient = new Client(clientReader, clientWriter);
                    SetOnlinePlay();
                    break;
            }
    }

    private void SetOnlinePlay()
    {
        Game.Helper.OnlineHelper();
        Game.Messages.SetClients(FirstClient,SecondClient);
        Game.GameNotifications.Helper.OnlineHelper();
        Game.NewHand();
    }
    private void ThreadProc(object obj)
        {
            object[] param = obj as object[];
            var client = (TcpClient)param[0];
            NetworkStream ns = client.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            var first = reader.ReadLine();
            if (first == "Code:sucess")
            {
                Console.WriteLine("Conexión con el jugador correcta");
            }

            SetClient(param[1].ToString(), reader, writer);
            Console.ReadLine();
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
                TcpClient tcpClientManager = Listener.AcceptTcpClient();
                Console.Write($"Cliente {NumOfConnectedPlayers + 1} Conectado!\n");
                ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { tcpClientManager, NumOfConnectedPlayers });
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
