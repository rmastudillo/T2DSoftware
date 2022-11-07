// See https://aka.ms/new-console-template for more information
using FrontendMessages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");
var msg = new Messages();

msg.WelcomeMessage();
msg.ShuffleCards(14);