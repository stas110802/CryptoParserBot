using Newtonsoft.Json;

namespace CryptoParserBot.AdditionalToolLibrary
{
    public static class JsonHelper
    {
        public static T? GetDeserializeObject<T>(string path)
            where T : class
        {
            var jsonData = File.ReadAllText(path);
            var result = JsonConvert.DeserializeObject<T>(jsonData);

            return result;  
        }
        
        public static void WriteDataAt<T>(string filePath, T data)
        {
            PathHelper.CheckForFileExists(filePath);
            var jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
        }
    }
}
