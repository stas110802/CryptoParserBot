namespace CryptoParserBot.AdditionalToolLibrary;

public sealed class PathList
{
    public string OrderPath => $"{LogsPath}orders\\";
    public string ErrorsPath => $"{LogsPath}errors\\";
    public string LaunchesPath => $"{LogsPath}launches\\";
    public string ConfigsPath => $"{ProjectPath}\\configs\\";
    private string LogsPath => $"{ProjectPath}\\logs\\";
    private string ProjectPath =>
        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\.."));// ..\

    public string GetProjectPath()
    {
        return ProjectPath;
    }
}