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
            if (Directory.Exists(path)) continue;

            Directory.CreateDirectory(path);
        }
    }
    
    /// <summary>
    /// Create files
    /// </summary>
    /// <param name="filePaths"></param>
    public static void CheckForFileExists(params string[] filePaths)
    {
        foreach (var path in filePaths)
        {
            if (File.Exists(path)) continue;

            using var file = File.Create(path);
            file.Close();
        }
    }
}