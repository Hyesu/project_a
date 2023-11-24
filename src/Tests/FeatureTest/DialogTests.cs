using Feature.Dialog;
using FeatureTest.Extensions;

namespace FeatureTest;

public class DialogTests : FeatureTestBase
{
    private class TestDlgHandler : IDialogHandler
    {
        public bool Started { get; private set; }
        public int UpdatedCnt { get; private set; }
        public bool Ended { get; private set; }

        public void OnStarted(DialogContext ctx)
        {
            Started = true;
        }

        public void OnUpdated(DialogContext ctx)
        {
            UpdatedCnt++;
        }

        public void OnEnded(DialogContext ctx)
        {
            Ended = true;
        }
    }

    private class TestDlgParticipant : IDialogParticipant
    {
        public readonly string Key;

        public bool Started { get; private set; }
        public bool Ended { get; private set; }
        public bool Active { get; private set; }

        public TestDlgParticipant(string key)
        {
            Key = key;
        }

        public string GetParticipantKey()
        {
            return Key;
        }

        public void OnStarted(DialogContext ctx)
        {
            Started = true;
        }

        public void OnActivated(DialogContext ctx)
        {
            Active = true;
        }

        public void OnDeactivated(DialogContext ctx)
        {
            Active = false;
        }

        public void OnEnded(DialogContext ctx)
        {
            Ended = true;
        }
    }

    //////////////////////////
    [Test]
    public void TestNext()
    {
        var dDlg = D.RandomDialog();
        var handler = new TestDlgHandler();
        var participants = dDlg.Speeches
            .DistinctBy(x => x.Character)
            .Select(x => new TestDlgParticipant(x.Character))
            .ToList();

        var context = DialogBuilder.Of(dDlg)
            .AddHandler(handler)
            .AddParticipants(participants)
            .Build();

        // check initial state
        Assert.That(context, Is.Not.Null);
        Assert.That(context.ActiveSpeech, Is.Null);
        Assert.That(context.ActiveIdx, Is.EqualTo(DialogContext.InactiveIdx));

        Assert.That(handler.Started, Is.False);
        Assert.That(handler.UpdatedCnt, Is.EqualTo(0));
        Assert.That(handler.Ended, Is.False);

        foreach (var participant in participants)
        {
            Assert.That(participant.Started, Is.False);
            Assert.That(participant.Active, Is.False);
            Assert.That(participant.Ended, Is.False);
        }

        // start => played first speech
        context.Start();
        Assert.That(context.ActiveSpeech, Is.EqualTo(dDlg.Speeches.First()));
        Assert.That(context.ActiveIdx, Is.EqualTo(0));

        Assert.That(handler.Started, Is.True);
        Assert.That(handler.UpdatedCnt, Is.EqualTo(1));
        Assert.That(handler.Ended, Is.False);

        var expectedParticipant = context.FindParticipant(dDlg.Speeches.First().Character);
        foreach (var participant in participants)
        {
            Assert.That(participant.Started, Is.True, $"participant({participant.GetParticipantKey()})");
            Assert.That(participant.Active, Is.EqualTo(expectedParticipant == participant));
            Assert.That(participant.Ended, Is.False);
        }

        // next => played second speech
        var playing = context.Next();
        Assert.That(playing, Is.EqualTo(dDlg.Speeches.Count > 1));
        Assert.That(context.ActiveSpeech, Is.EqualTo(dDlg.Speeches[1]));
        Assert.That(context.ActiveIdx, Is.EqualTo(1));

        // next until end => check finished state
        while (context.Next()) { }
        Assert.That(context.ActiveSpeech, Is.EqualTo(null));
        Assert.That(context.ActiveIdx, Is.EqualTo(dDlg.Speeches.Count));

        Assert.That(handler.Started, Is.True);
        Assert.That(handler.UpdatedCnt, Is.EqualTo(dDlg.Speeches.Count));
        Assert.That(handler.Ended, Is.True);
    }

    [Test]
    public void TestJump()
    {
        var dDlg = D.RandomDialog();
        var context = DialogBuilder.Of(dDlg)
            .Build();

        context.Start();

        var idx = Random.Shared.Next(2, dDlg.Speeches.Count);
        var playing = context.Jump(idx);
        Assert.That(playing, Is.True);
        Assert.That(context.ActiveSpeech, Is.EqualTo(dDlg.Speeches[idx]));
        Assert.That(context.ActiveIdx, Is.EqualTo(idx));
    }

    [Test]
    public void TestEnd()
    {
        var dDlg = D.RandomDialog();
        var context = DialogBuilder.Of(dDlg)
            .Build();

        context.Start();

        context.End();
        Assert.That(context.ActiveSpeech, Is.Null);
        Assert.That(context.ActiveIdx, Is.EqualTo(dDlg.Speeches.Count));
    }
}