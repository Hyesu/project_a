using Newtonsoft.Json.Linq;
using Encyclopedia.Core;
using Encyclopedia.Entrys;

namespace Encyclopedia.Sections;

public class EncyDialogSection : EncySection
{
    public EncyDialogSection(string path)
        : base(nameof(EncyDialogSection), path)
    {
    }

    protected override EncyEntry CreateEntry(JObject jsonObj)
    {
        return new EncyDialogEntry(jsonObj);
    }
}
