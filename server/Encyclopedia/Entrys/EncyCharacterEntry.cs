using Newtonsoft.Json.Linq;
using Encyclopedia.Core;
using Encyclopedia.Extensions;

namespace Encyclopedia.Entrys;

public class EncyCharacterEntry : EncyEntry
{
    public readonly string Name;
    public readonly string Desc;
    public readonly string PortraitPath;

    public EncyCharacterEntry(JObject json)
        : base(json)
    {
        Name = json.GetString("Name");
        Desc = json.GetString("Desc");
        PortraitPath = json.GetString("PortraitPath");
    }
}