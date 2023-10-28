using Newtonsoft.Json.Linq;
using Encyclopedia.Core;
using Encyclopedia.Entrys;

namespace Encyclopedia.Sections;

public class EncyCharacterSection : EncySection
{
    public EncyCharacterSection(string path)
        : base(nameof(EncyCharacterSection), path)
    {
    }

    protected override EncyEntry CreateEntry(JObject jsonObj)
    {
        return new EncyCharacterEntry(jsonObj);
    }
}
