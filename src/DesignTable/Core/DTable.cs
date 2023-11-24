using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DesignTable.Core
{
    public class DTable
    {
        private readonly string _name;
        private readonly string _path;

        private readonly Dictionary<int, DEntry> _entries;
        private readonly Dictionary<string, DEntry> _entriesByStrId;

        public string Name => _name;
        public string Path => _path;
        public IEnumerable<DEntry> All => _entries.Values;

        public DTable(string name, string path)
        {
            _name = name;
            _path = path;

            _entries = new();
            _entriesByStrId = new();
        }

        protected virtual DEntry CreateEntry(JObject entry)
        {
            throw new NotImplementedException($"not implemented ency-section entry creator");
        }

        public virtual void Initialize(IList<JObject> jsonObjs)
        {
            var entries = jsonObjs.Select(x => CreateEntry(x));
            foreach (var entry in entries)
            {
                AddEntry(entry);
            }
        }

        public virtual void PostInitialize(IReadOnlyDictionary<Type, DTable> allSections)
        {
        }

        protected T GetInternal<T>(int id) where T : DEntry
        {
            if (!_entries.TryGetValue(id, out var entry))
                return null;

            return entry as T;
        }

        protected T GetByStrIdInternal<T>(string strId) where T : DEntry
        {
            if (!_entriesByStrId.TryGetValue(strId, out var entry))
                return null;

            return entry as T;
        }

        private void AddEntry(DEntry entry)
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
}