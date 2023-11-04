using DesignTable.Entry;

namespace Common.Dialog;

public class DialogContext
{
    // static
    public static DialogContext Make()
    {
        return null;
    }

    // non-static
    public readonly DDialog D;

    private int _activeIdx;

    public DialogContext(DDialog dDlg)
    {
        D = dDlg;
        _activeIdx = -1;
    }

    public void Start()
    {
        _activeIdx = 0;
    }

    public void Next()
    {

    }
}