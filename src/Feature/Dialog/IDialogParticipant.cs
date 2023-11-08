namespace Feature.Dialog;

public interface IDialogParticipant
{
    string GetParticipantKey();

    void OnStarted(DialogContext ctx);
    void OnActivated(DialogContext ctx);
    void OnDeactivated(DialogContext ctx);
    void OnEnded(DialogContext ctx);
}
