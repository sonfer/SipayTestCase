using Newtonsoft.Json;

namespace SipayTestCase.Shared.Helpers;

public static class JsonFileReadHelper
{
    public async static Task<T?> ReadAsync<T>(string fileName)
    {
        var fullPath = GetFullPath(fileName);
        using StreamReader reader = new StreamReader(fullPath);
        var json =await  reader.ReadToEndAsync();
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore, 
            MissingMemberHandling = MissingMemberHandling.Ignore,
            
        };
        var data = JsonConvert.DeserializeObject<T>(json, settings);
        return data;
    }

    public static string GetFullPath(string fileName)
    {
        var dirPath = System.IO.Directory.GetCurrentDirectory();
        dirPath = Path.GetFullPath(dirPath);
        var fullPath=  Path.GetFullPath(Path.Combine(dirPath, fileName));
        return fullPath;
    }
}