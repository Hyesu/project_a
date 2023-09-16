using Newtonsoft.Json.Linq;

namespace Encyclopedia.Core;

public class EncySection
{
    private readonly string _name;
    private readonly string _path;

    private readonly Dictionary<int, EncyEntry> _entries;
    private readonly Dictionary<string, EncyEntry> _entriesByStrId;

    public string Name => _name;
    public string Path => _path;
    public IEnumerable<EncyEntry> All => _entries.Values;

    public EncySection(string name, string path)
    {
        _name = name;
        _path = path;

        _entries = new();
        _entriesByStrId = new();
    }

    public virtual void Initialize(IList<JObject> entryObjs)
    {
        throw new NotImplementedException($"not implemented ency-section");
    }

    public virtual void PostInitialize(IReadOnlyDictionary<Type, EncySection> allSections)
    {
    }

    public T? Get<T>(int id) where T : EncyEntry
    {
        if (!_entries.TryGetValue(id, out var entry))
            return null;

        return entry as T;
    }

    public T? GetByStrId<T>(string strId) where T : EncyEntry
    {
        if (!_entriesByStrId.TryGetValue(strId, out var entry))
            return null;

        return entry as T;
    }

    protected void AddEntry(EncyEntry entry)
    {
        if (_entries.ContainsKey(entry.Id))
        {
            throw new InvalidDataException($"duplicate ency-entry id- section({_name}) id({entry.Id})");
        }

        if (_entriesByStrId.ContainsKey(entry.StrId))
        {
            throw new InvalidDataException($"duplicate ency-entry strId- section({_name}) strId({entry.StrId})");
        }

        _entries.Add(entry.Id, entry);
        _entriesByStrId.Add(entry.StrId, entry);
    }
}