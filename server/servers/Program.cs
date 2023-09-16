using System.Configuration;
using Encyclopedia.Core;
using Encyclopedia.Entrys;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("==================== Start Server ====================");

        var cwd = Directory.GetCurrentDirectory();

        // TODO: 서버 클래스 만들 때 여기서 setting만 넘겨주고, 서버 내부에서 초기화하게 수정
        // load encyclopedia
        var encyRoot = ConfigurationManager.AppSettings["EncyclopediaRoot"];
        if (null == encyRoot)
            throw new InvalidDataException($"not found app setting - EncyclopediaRoot");

        var encyclopedia = new EncyBody(encyRoot);
        encyclopedia.Initialize();

        // test code
        foreach (var section in encyclopedia.Sections)
        {
            foreach (var entry in section.All)
            {
                Console.WriteLine($"table({section.Name}) - id({entry.Id}) strId({entry.StrId})");
            }
        }

        var me = encyclopedia.Character.GetByStrId<EncyCharacterEntry>("char_me");
        Console.WriteLine($"-- me: name({me.Name})");
    }
}