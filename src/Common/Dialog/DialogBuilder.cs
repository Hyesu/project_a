using DesignTable.Entry;

namespace Feature.Dialog
{
    public class DialogBuilder
    {
        public static DialogBuilder Of(DDialog dDlg)
        {
            return new DialogBuilder(dDlg);
        }

        private DDialog _dDlg;
        private List<IDialogParticipant> _participants;
        private List<IDialogHandler> _handlers;

        public DialogBuilder(DDialog dDlg)
        {
            _dDlg = dDlg;
            _participants = new();
            _handlers = new();
        }

        public DialogBuilder AddParticipant(IDialogParticipant participant)
        {
            _participants.Add(participant);
            return this;
        }

        public DialogBuilder AddParticipants(IEnumerable<IDialogParticipant> participants)
        {
            _participants.AddRange(participants);
            return this;
        }

        public DialogBuilder AddHandler(IDialogHandler handler)
        {
            _handlers.Add(handler);
            return this;
        }

        public DialogContext Build()
        {
            return new DialogContext(_dDlg, _participants, _handlers);
        }
    }
}
