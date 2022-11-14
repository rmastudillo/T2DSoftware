using System.Net.Security;

namespace Escoba;
using System.Net.Sockets;
using System;
using System.Net;


public class Client
{
    private StreamWriter Writer { get; set; }
    private StreamReader Reader { get; set; }
    private TcpClient _clientConnection = new TcpClient();
    private bool _writeMode = false;
    private bool _playing = true;
    
    public Client()
    {
        _clientConnection.Connect(IPAddress.Loopback, 8000);
        Console.Write($"Conectado!\n");
        var networkStream = _clientConnection.GetStream();
        Writer = new StreamWriter(networkStream);
        Reader = new StreamReader(networkStream);
    }

    private void ShowServerMessages(TextReader reader)
    {
        var response = reader.ReadLine();
        while (response!="" && !response.Contains("Code:"))
        {
            Console.WriteLine(response);
            response = reader.ReadLine();
        }
        switch (response)
        {
            case "Code:input":
                _writeMode = true;
                break;
            case "Code:end":
                _playing = false;
                break;
        }
    }

    private void SendMessage(string message)
    {
        Writer.WriteLine(message);
        Writer.Flush();
    }
    public void StartConnection()
    {
        Console.WriteLine("Conexión establecida correctamente\nIniciando el juego\n\n");
        SendMessage("Hello There");
        while (_playing)
        {
            
            ShowServerMessages(Reader);
            if (_writeMode)
            {
                var asdas = Console.ReadLine();
                SendMessage("¿Que quieres hacer?\n[1] Enviar mensaje\n[2] Salir\n");
                _writeMode = false;
            }
        }
        Writer.WriteLine("Salir");
        Writer.Flush();
        _clientConnection.Close();
    }
}