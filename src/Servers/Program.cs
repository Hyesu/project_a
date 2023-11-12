using System.Configuration;
using DesignTable.Core;

Console.WriteLine("==================== Start Server ====================");

// TODO: app config가 아닌 별도 파일 통해서 setting 설정할 수 있도록 수정 필요
// load design table
var ddRoot = ConfigurationManager.AppSettings["DesignDataRoot"];
if (null == ddRoot)
    throw new InvalidDataException($"not found app setting - DesignDataRoot");

var dt = new DContext(ddRoot);
dt.Initialize();

// test code
foreach (var section in dt.Sections)
{
    foreach (var entry in section.All)
    {
        Console.WriteLine($"table({section.Name}) - id({entry.Id}) strId({entry.StrId})");
    }
}

var dlg = dt.Dialog.GetByStrId("dlg_sample");
Console.WriteLine($"dlg({dlg.StrId}) type({dlg.Type})");
foreach (var speech in dlg.Speeches)
{
    Console.WriteLine($"-- {speech.Character} : {speech.Text} // emotion({speech.Emotion})");
}