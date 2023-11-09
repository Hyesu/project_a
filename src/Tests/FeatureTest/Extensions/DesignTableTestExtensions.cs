using DesignTable.Core;
using DesignTable.Entry;
using DesignTable.Table;

namespace FeatureTest.Extensions;

public static class DesignTableExtensions
{
    public static DDialog RandomDialog(this DContext ctx)
    {
        var dlgTable = ctx.Get<DDialogTable>();
        var dlgs = dlgTable.All
            .Select(x => x as DDialog)
            .Where(x => !x.Speeches.Any(y => !string.IsNullOrEmpty(y.JumpKey)))
            .ToList();

        return dlgs.ElementAt(Random.Shared.Next(dlgs.Count));
    }
}