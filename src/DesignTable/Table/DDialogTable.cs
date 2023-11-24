using Newtonsoft.Json.Linq;
using DesignTable.Core;
using DesignTable.Entry;

namespace DesignTable.Table
{
    public class DDialogTable : DTable
    {
        public DDialogTable(string path)
            : base(nameof(DDialogTable), path)
        {
        }

        protected override DEntry CreateEntry(JObject jsonObj)
        {
            return new DDialog(jsonObj);
        }

        public DDialog Get(int id)
        {
            return GetInternal<DDialog>(id);
        }

        public DDialog GetByStrId(string strId)
        {
            return GetByStrIdInternal<DDialog>(strId);
        }
    }
   
}