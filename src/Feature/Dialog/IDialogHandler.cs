namespace Feature.Dialog;

public interface IDialogHandler
{
    void OnStarted(DialogContext ctx);
    void OnUpdated(DialogContext ctx);
    void OnEnded(DialogContext ctx);
}
