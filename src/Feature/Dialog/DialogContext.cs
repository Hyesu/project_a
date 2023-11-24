using System.Collections.Immutable;
using DesignTable.Entry;

namespace Feature.Dialog;

public class DialogContext
{
    public const int InactiveIdx = -1;

    private readonly DDialog _dDlg;
    private readonly Dictionary<string, IDialogParticipant> _participants;
    private readonly ImmutableArray<IDialogHandler> _handlers;
    private bool _hasStarted;

    public DDialogSpeech ActiveSpeech { get; private set; }
    public int ActiveIdx { get; private set; }

    public DialogContext(DDialog dDlg, IEnumerable<IDialogParticipant> participants, IEnumerable<IDialogHandler> handlers)
    {
        _dDlg = dDlg;
        _participants = participants.ToDictionary(x => x.GetParticipantKey(), x => x);
        _handlers = handlers.ToList().ToImmutableArray();

        ActiveSpeech = null;
        ActiveIdx = InactiveIdx;
    }

    private void Reset()
    {
        ActiveIdx = InactiveIdx;
        ActiveSpeech = null;
    }

    private void Activate(DDialogSpeech rSpeech)
    {
        if (null == rSpeech)
        {
            return;
        }

        ActiveSpeech = rSpeech;

        var participant = FindParticipant(rSpeech.Character);
        participant?.OnActivated(this);

        foreach (var handler in _handlers)
        {
            handler.OnUpdated(this);
        }
    }

    private void Deactivate()
    {
        if (null == ActiveSpeech)
        {
            return;
        }

        var prevSpeech = ActiveSpeech;
        ActiveSpeech = null;

        var participant = FindParticipant(prevSpeech.Character);
        participant?.OnDeactivated(this);
    }

    public IDialogParticipant FindParticipant(string key)
    {
        return _participants.TryGetValue(key, out var participant) ? participant : null;
    }

    public void Start()
    {
        if (_hasStarted)
        {
            throw new InvalidOperationException($"cannot start duplicately for started dialog - dDlg({_dDlg.StrId})");
        }

        Reset();

        foreach (var participant in _participants.Values)
        {
            participant.OnStarted(this);
        }

        foreach (var handler in _handlers)
        {
            handler.OnStarted(this);
        }

        _hasStarted = true;

        Next();
    }

    public bool Next()
    {
        if (!_hasStarted)
        {
            throw new InvalidOperationException($"cannot next for not started dialog - dDlg({_dDlg.StrId})");
        }

        var nextIdx = ActiveIdx + 1;
        var jumpKey = ActiveSpeech?.JumpKey;
        var jumpSpeech = _dDlg.FindSpeech(jumpKey);
        if (null != jumpSpeech)
        {
            nextIdx = _dDlg.Speeches.IndexOf(jumpSpeech);
        }

        return Jump(nextIdx);
    }

    public bool Jump(int idx)
    {
        if (!_hasStarted)
        {
            throw new InvalidOperationException($"cannot jump for not started dialog - dDlg({_dDlg.StrId})");
        }

        Deactivate();

        ActiveIdx = idx;

        var nextSpeech = _dDlg.Speeches.ElementAtOrDefault(ActiveIdx);
        if (null != nextSpeech)
        {
            Activate(nextSpeech);
            return true;
        }

        End();
        return false;
    }

    public void End()
    {
        if (!_hasStarted)
        {
            throw new InvalidOperationException($"cannot end for not started dialog - dDlg({_dDlg.StrId})");
        }

        Deactivate();

        ActiveIdx = _dDlg.Speeches.Count;

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