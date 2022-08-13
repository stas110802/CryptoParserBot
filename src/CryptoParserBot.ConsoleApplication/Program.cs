using CryptoParserBot.ConsoleApplication;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var startUp = new StartUp();
startUp.CheckFoldersExists();
startUp.PrintStartUpMessage();
startUp.ReadCommands();