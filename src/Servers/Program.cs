using System.Configuration;
using DesignTable.Core;
using DesignTable.Entry;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("==================== Start Server ====================");

        var cwd = Directory.GetCurrentDirectory();

        // TODO: 서버 클래스 만들 때 여기서 setting만 넘겨주고, 서버 내부에서 초기화하게 수정
        // load encyclopedia
        var ddRoot = ConfigurationManager.AppSettings["DesignDataRoot"];
        if (null == ddRoot)
            throw new InvalidDataException($"not found app setting - DesginDataRoot");

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
    }
}