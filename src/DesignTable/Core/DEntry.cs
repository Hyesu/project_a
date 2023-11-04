using DesignTable.Extensions;
using Newtonsoft.Json.Linq;

namespace DesignTable.Core;

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