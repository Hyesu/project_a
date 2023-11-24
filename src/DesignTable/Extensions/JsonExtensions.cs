using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DesignTable.Extensions
{
    public static class JsonExtensions
    {
        public static int GetInt(this JObject json, string key)
        {
            return int.Parse(json.GetString(key));
        }

        public static string GetString(this JObject json, string key)
        {
            if (!json.TryGetValue(key, out var value))
            {
                return null;
            }

            return value.ToString();
        }

        public static T GetEnum<T>(this JObject json, string key) where T : Enum
        {
            return (T)Enum.Parse(typeof(T),json.GetString(key));
        }

        public static IEnumerable<JObject> GetObjArray(this JObject json, string key)
        {
            if (!json.TryGetValue(key, out var value))
            {
                return Enumerable.Empty<JObject>();
            }

            return value.ToArray()
                .Select(x => x.ToObject<JObject>());
        }
    }   
}