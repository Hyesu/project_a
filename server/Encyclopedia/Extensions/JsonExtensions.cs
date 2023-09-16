using Newtonsoft.Json.Linq;

namespace Encyclopedia.Extensions;

public static class JsonExtensions
{
    public static int GetInt(this JObject json, string key)
    {
        return int.Parse(json.GetString(key));
    }

    public static string GetString(this JObject json, string key)
    {
        return json[key].ToString();
    }
}