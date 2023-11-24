using DesignTable.Core;
using DesignTable.Extensions;
using Newtonsoft.Json.Linq;

namespace DesignTable.Entry
{
    public class DCharacter : DEntry
    {
        public readonly string Name;
        public readonly string Desc;
        public readonly string PortraitPath;

        public DCharacter(JObject json)
            : base(json)
        {
            Name = json.GetString("Name");
            Desc = json.GetString("Desc");
            PortraitPath = json.GetString("PortraitPath");
        }
    }
}