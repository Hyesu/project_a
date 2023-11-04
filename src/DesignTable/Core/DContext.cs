using Newtonsoft.Json.Linq;
using DesignTable.Table;

namespace DesignTable.Core;

public class DContext
{
    private readonly string _rootPath;
    private readonly Dictionary<Type, DTable> _tables;

    public IEnumerable<DTable> Sections => _tables.Values;

    // indexes ///////////////////
    public readonly DCharacterTable Character;
    public readonly DDialogTable Dialog;
    //////////////////////////////

    public DContext(string rootPath)
    {
        _rootPath = rootPath;
        _tables = new();

        // create indexes
        Character = AddTable(new DCharacterTable("Character"));
        Dialog = AddTable(new DDialogTable("Dialog"));
    }

    public void Initialize()
    {
        var sectionTasks = _tables.Values
            .Select(x => LoadTableAsync(x));

        Task.WaitAll(sectionTasks.ToArray());

        foreach (var section in _tables.Values)
        {
            section.PostInitialize(_tables);
        }
    }

    private async Task<string> LoadTableAsync(DTable table)
    {
        var tablePath = _rootPath + table.Path;
        var filePaths = Directory.EnumerateFiles(tablePath, "*.json");
        var jsonObjs = new List<JObject>();
        foreach (var filePath in filePaths)
        {
            using (var sr = new StreamReader(filePath))
            {
                string json = await sr.ReadToEndAsync();
                var jsonObj = JObject.Parse(json);
                jsonObjs.Add(jsonObj);
            }
        }

        table.Initialize(jsonObjs);
        return table.Name;
    }

    protected T AddTable<T>(T table) where T : DTable
    {
        _tables.Add(typeof(T), table);
        return table;
    }

}
