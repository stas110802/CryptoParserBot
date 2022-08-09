using CryptoParserBot.ConsoleApplication;

var startUp = new StartUp();
startUp.CheckFoldersExists();
startUp.PrintStartUpMessage();
startUp.PrintCommands();
startUp.ReadCommands();