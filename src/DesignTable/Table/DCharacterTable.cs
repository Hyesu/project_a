using Newtonsoft.Json.Linq;
using DesignTable.Core;
using DesignTable.Entry;

namespace DesignTable.Table
{
    public class DCharacterTable : DTable
    {
        public DCharacterTable(string path)
            : base(nameof(DCharacterTable), path)
        {
        }

        protected override DEntry CreateEntry(JObject jsonObj)
        {
            return new DCharacter(jsonObj);
        }

        public DCharacter Get(int id)
        {
            return GetInternal<DCharacter>(id);
        }

        public  DCharacter GetByStrId(string strId)
        {
            return GetByStrIdInternal<DCharacter>(strId);
        }
    }
   
}