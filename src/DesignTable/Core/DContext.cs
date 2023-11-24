using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DesignTable.Table;

namespace DesignTable.Core
{
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
            Character = Add(new DCharacterTable("Character"));
            Dialog = Add(new DDialogTable("Dialog"));
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

        private T Add<T>(T table) where T : DTable
        {
            _tables.Add(typeof(T), table);
            return table;
        }

        public T Get<T>() where T : DTable
        {
            return _tables.TryGetValue(typeof(T), out var table) ? table as T : null;
        }
    }
}