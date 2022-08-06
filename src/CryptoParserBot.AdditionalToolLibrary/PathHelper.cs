﻿namespace CryptoParserBot.AdditionalToolLibrary;

public static class PathHelper
{
    static PathHelper()
    {
        CheckForPathExists(LogsPath, OrderPath, ErrorsPath, LaunchesPath);
    }
    public static string ProjectPath =>
        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\.."));

    private static string LogsPath => $"{ProjectPath}\\logs\\";
    public static string OrderPath => $"{LogsPath}orders\\";
    public static string ErrorsPath => $"{LogsPath}errors\\";
    public static string LaunchesPath => $"{LogsPath}launches\\";
    
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