using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using Encyclopedia.Core;
using Encyclopedia.Extensions;

namespace Encyclopedia.Entrys;

public enum DialogType
{
    None,
    Conversation,
}

public class EncyDialogChoice
{
    public readonly string Text;
    public readonly string Key;

    public EncyDialogChoice(JObject json)
    {
        Text = json.GetString("Text");
        Key = json.GetString("Key");
    }
}

public class EncyDialogSpeech
{
    public readonly string Key;
    public readonly string Character;
    public readonly string Pivot;
    public readonly string Emotion;
    public readonly string Text;

    // 대사 분기
    public readonly string JumpKey;
    public readonly ImmutableArray<EncyDialogChoice> Choices;

    public EncyDialogSpeech(JObject json)
    {
        Key = json.GetString("Key") ?? string.Empty;
        Character = json.GetString("Character");
        Pivot = json.GetString("Pivot") ?? "Center";
        Emotion = json.GetString("Emotion") ?? "Idle";
        Text = json.GetString("Text") ?? string.Empty;

        JumpKey = json.GetString("JumpKey") ?? string.Empty;
        Choices = json.GetObjArray("Choices")
            .Select(x => new EncyDialogChoice(x))
            .ToImmutableArray();
    }
}

public class EncyDialogEntry : EncyEntry
{
    public readonly DialogType Type;
    public readonly ImmutableArray<EncyDialogSpeech> Speeches;

    public EncyDialogEntry(JObject json)
        : base(json)
    {
        Type = json.GetEnum<DialogType>("Type");
        Speeches = json.GetObjArray("Speeches")
            .Select(x => new EncyDialogSpeech(x))
            .ToImmutableArray();
    }
}