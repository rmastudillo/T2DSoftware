using System.Net;
using System.Net.Sockets;

void SendMessage(StreamReader reader, StreamWriter writer, TcpClient client)
{
    Console.WriteLine("¿Que quieres hacer?\n[1] Enviar mensaje\n[2] Salir\n");
    var opcion = Console.ReadLine();
    while (opcion != "2")
    {
        Console.WriteLine("Escribe el mensaje a enviar:");
        writer.WriteLine(Console.ReadLine());
        writer.Flush();
        string response = reader.ReadLine();
        Console.WriteLine("El servidor dice: " + response);
        Console.WriteLine("¿Que quieres hacer?\n[1] Enviar mensaje\n[2] Salir\n");
        opcion = Console.ReadLine();
    }
    writer.WriteLine("Salir");
    writer.Flush();
    client.Close();
}

TcpClient client = new TcpClient();
client.Connect(IPAddress.Loopback, 8000);
Console.Write($"Conectado!\n");

NetworkStream ns = client.GetStream();
StreamWriter writer = new StreamWriter(ns);
StreamReader reader = new StreamReader(ns);
SendMessage(reader, writer, client);