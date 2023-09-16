using Newtonsoft.Json.Linq;

namespace Encyclopedia.Core;

public class EncyEntry
{
    protected readonly int _id;
    protected readonly string _strId;

    public int Id => _id;
    public string StrId => _strId;

    public EncyEntry(int id, string strId)
    {
        _id = id;
        _strId = strId;
    }

    public virtual void Initialize(JObject entryObj)
    {
        throw new NotImplementedException($"not implemented ency-entry");
    }
}