using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoParserBot.CryptoBot.Models;
using Newtonsoft.Json;

namespace CryptoParserBot.CryptoBot.Logs;

public sealed class JsonLogger
{
    /// <summary>
    /// write log info to json file
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="logInfo"></param>
    public void LogInfoAt<T>(string filePath, T logInfo)
    {
        // if the file not exists
        if (File.Exists(filePath) is false)
        {
            // create new file
            using var file = File.Create(filePath);
            file.Close();
        }
        
        // Read existing json data
        var jsonData = File.ReadAllText(filePath);

        // De-serialize to object or create new list
        var logList = JsonConvert.DeserializeObject<List<T>>(jsonData)
                      ?? new List<T>();
            
        logList.Add(logInfo);

        // Update json data string
        jsonData = JsonConvert.SerializeObject(logList, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }
}