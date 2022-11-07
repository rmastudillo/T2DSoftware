// See https://aka.ms/new-console-template for more information
using FrontendMessages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
var msg = new Messages();

msg.WelcomeMessage();
// msg.ShuffleCards(14);
// msg.Escoba(1);
msg.HandStats(new List<string> { "Caballo_Espada", "6_Bastos", "Rey_Oro", "5_Espada", "3_Espada", "Caballo_Bastos" }, new List<string> { "1_Oro", "Rey_Bastos", "1_Copa", "3_Oro", "5_Bastos", "Rey_Copa", "2_Oro", "7_Oro", "6_Copa", "2_Copa", "4_Espada", "Caballo_Oro", "5_Oro", "Rey_Espada", "3_Copa", "Sota_Oro", "4_Bastos", "1_Bastos" });