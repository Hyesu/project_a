using Encyclopedia.Extensions;
using Newtonsoft.Json.Linq;

namespace Encyclopedia.Core;

public class EncyEntry
{
    public readonly int Id;
    public readonly string StrId;

    public EncyEntry(JObject json)
    {
        Id = json.GetInt("Id");
        StrId = json.GetString("StrId");
    }

    public virtual void Initialize(JObject entryObj)
    {
        throw new NotImplementedException($"not implemented ency-entry");
    }
}