using CryptoParserBot.ConsoleApplication;
using CryptoParserBot.ConsoleApplication.Commands;
using CryptoParserBot.ExchangeClients.Clients;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var startUp = new StartUp();
startUp.CheckFoldersExists();
startUp.PrintStartUpMessage();
MainCommands.PrintCommands();
startUp.ReadCommands();