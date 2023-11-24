using System;
using Newtonsoft.Json.Linq;
using DesignTable.Extensions;

namespace DesignTable.Core
{
    public class DEntry
    {
        public readonly int Id;
        public readonly string StrId;

        public DEntry(JObject json)
        {
            Id = json.GetInt("Id");
            StrId = json.GetString("StrId");
        }

        public virtual void Initialize(JObject entryObj)
        {
            throw new NotImplementedException($"not implemented ency-entry");
        }
    }
}