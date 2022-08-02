using Newtonsoft.Json;

namespace CryptoParserBot.AdditionalToolLibrary
{
    public static class JsonHelper
    {
        public static T GetDeserializeObject<T>(string path)
            where T : class
        {
            // Read existing json dataConfigPath
            var jsonData = File.ReadAllText(path);
            // De-serialize to object or create new list
            var result = JsonConvert.DeserializeObject<T>(jsonData);

            if (result == null)
                throw new Exception("File not found.");

            return result;
        }
    }
}
