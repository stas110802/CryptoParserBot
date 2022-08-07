using CryptoParserBot.ConsoleApplication;
using CryptoParserBot.ConsoleApplication.Commands;

var startUp = new StartUp();
startUp.CheckFoldersExists();
startUp.PrintStartUpMessage();
startUp.PrintCommands();
startUp.ReadCommands();





