namespace CryptoParserBot.AdditionalToolLibrary;

public static class PathHelper
{
    public static string ProjectPath =>
        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\.."));
    /// <summary>
    /// Create folders
    /// </summary>
    /// <param name="filePaths"></param>
    public static void CheckForPathExists(params string[] filePaths)
    {
        foreach (var path in filePaths)
        {
            if (Directory.Exists(path) is true) continue;
            
            var file = Directory.CreateDirectory(path);
        }
    }
}