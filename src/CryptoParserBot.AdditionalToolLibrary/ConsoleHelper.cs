namespace CryptoParserBot.AdditionalToolLibrary;

public static class ConsoleHelper
{
    private const string Border = "\u2551";
    private const int Max = 20;

    public static void LoadingBar(int sec, string text = "updating data", int x = 1, int y = 5)
    {
        var thrSleep = sec * 1000 / Max;
        var empty = new string(' ', Max);
        
        Console.CursorVisible = false;
        Console.SetCursorPosition(1, 1);
        
        for (var i = 0; i < Max; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;// bar color
            Console.SetCursorPosition(x, y);
        
            for (var j = 0; j < i; j++)
            {
                Console.Write(Border);
            }
            
            Console.Write(empty + (i + 1) + $" / {Max} {text}...");
            empty = empty.Remove(empty.Length - 1);
            Thread.Sleep(thrSleep);
        }

        Console.ForegroundColor = ConsoleColor.White;
    }
}