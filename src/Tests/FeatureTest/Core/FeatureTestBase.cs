using DesignTable.Core;

namespace FeatureTest;

public class FeatureTestBase
{
    public readonly DContext D;

    public FeatureTestBase()
    {
        D = new DContext("../../../../../../client/Assets/Resources/DesignDatas/");
        D.Initialize();
    }
}