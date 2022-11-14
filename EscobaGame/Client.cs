namespace Escoba;
using System.Net.Sockets;
using System;
using System.Net;


public class Client
{
    private StreamWriter Writer { get; set; }
    private StreamReader Reader { get; set; }
    private TcpClient ClientConnection = new TcpClient();
    private bool WriteMode = false;
    
    public Client()
    {
        ClientConnection.Connect(IPAddress.Loopback, 8000);
        Console.Write($"Conectado!\n");
        var networkStream = ClientConnection.GetStream();
        Writer = new StreamWriter(networkStream);
        Reader = new StreamReader(networkStream);
    }

    private void ShowServerMessages(TextReader reader)
    {
        var response = reader.ReadLine();
        while (response is not( "" or "input\n"))
        {
            Console.WriteLine(response);
            response = reader.ReadLine();
        }

        if (response is "input\n")
        {
            Console.WriteLine("ASDSAAAAAAAAAAAAAAAAA");
            WriteMode = true;
        }
    }

    public void StartConnection()
    {
        Console.WriteLine("¿Que quieres hacer?\n[1] Enviar mensaje\n[2] Salir\n");
        var opcion = Console.ReadLine();
        while (opcion != "2")
        {
            Console.WriteLine("Escribe el mensaje a enviar:");
            Writer.WriteLine(Console.ReadLine());
            Writer.Flush();
            ShowServerMessages(Reader);
            if (WriteMode)
            {
                Console.WriteLine("¿Que quieres hacer?\n[1] Enviar mensaje\n[2] Salir\n");
                opcion = Console.ReadLine();
            }
        }
        Writer.WriteLine("Salir");
        Writer.Flush();
        ClientConnection.Close();
    }
}