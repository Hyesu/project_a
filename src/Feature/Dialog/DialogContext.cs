using System.Collections.Immutable;
using DesignTable.Entry;

namespace Feature.Dialog;

public class DialogContext
{
    private readonly DDialog _dDlg;
    private readonly Dictionary<string, IDialogParticipant> _participants;
    private readonly ImmutableArray<IDialogHandler> _handlers;

    private DDialogSpeech _activeSpeech;
    private int _activeIdx;

    public DialogContext(DDialog dDlg, IEnumerable<IDialogParticipant> participants, IEnumerable<IDialogHandler> handlers)
    {
        _dDlg = dDlg;
        _participants = participants.ToDictionary(x => x.GetParticipantKey(), x => x);
        _handlers = handlers.ToList().ToImmutableArray();

        _activeSpeech = null;
        _activeIdx = -1;
    }

    private void Reset()
    {
        _activeIdx = -1;
        _activeSpeech = null;
    }

    private void Activate(DDialogSpeech rSpeech)
    {
        if (null == rSpeech)
        {
            return;
        }

        _activeSpeech = rSpeech;

        var participant = FindParticipant(rSpeech.Character);
        participant?.OnActivated(this);

        foreach (var handler in _handlers)
        {
            handler.OnUpdated(this);
        }
    }

    private void Deactivate()
    {
        if (null == _activeSpeech)
        {
            return;
        }

        var prevSpeech = _activeSpeech;
        _activeSpeech = null;

        var participant = FindParticipant(prevSpeech.Character);
        participant?.OnDeactivated(this);
    }

    public IDialogParticipant FindParticipant(string key)
    {
        return _participants.TryGetValue(key, out var participant) ? participant : null;
    }

    public void Start()
    {
        Reset();

        foreach (var participant in _participants.Values)
        {
            participant.OnStarted(this);
        }

        foreach (var handler in _handlers)
        {
            handler.OnStarted(this);
        }

        Next();
    }

    public void Next()
    {
        Jump(_activeIdx + 1);
    }

    public void Jump(int idx)
    {
        Deactivate();

        _activeIdx = idx;

        var nextSpeech = _dDlg.Speeches.ElementAtOrDefault(_activeIdx);
        if (null != nextSpeech)
        {
            Activate(nextSpeech);
            return;
        }

        End();
    }

    public void End()
    {
        Deactivate();

        _activeIdx = _dDlg.Speeches.Length;

        foreach (var participant in _participants.Values)
        {
            participant.OnEnded(this);
        }

        foreach (var handler in _handlers)
        {
            handler.OnEnded(this);
        }
    }
}