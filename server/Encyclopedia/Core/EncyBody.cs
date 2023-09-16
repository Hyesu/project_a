using Newtonsoft.Json.Linq;

namespace Encyclopedia.Core;

public class EncyBody
{
    private readonly string _rootPath;
    private readonly Dictionary<Type, EncySection> _sections;

    // indexes ///////////////////


    //////////////////////////////

    public EncyBody(string rootPath)
    {
        _rootPath = rootPath;
        _sections = new();
    }

    public void Initialize()
    {
        var sectionTasks = _sections.Values
            .Select(x => LoadSectionAsync(x));

        Task.WaitAll(sectionTasks.ToArray());

        foreach (var section in _sections.Values)
        {
            section.PostInitialize(_sections);
        }
    }

    private async Task<string> LoadSectionAsync(EncySection section)
    {
        var tablePath = _rootPath + section.Path;
        var filePaths = Directory.EnumerateFiles(tablePath);
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

        section.Initialize(jsonObjs);
        return section.Name;
    }
}
