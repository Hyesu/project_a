using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using DesignTable.Core;
using DesignTable.Extensions;

namespace DesignTable.Entry
{
    public enum DialogType
    {
        None,
        Conversation,
    }

    public class DDialogChoice
    {
        public readonly string Text;
        public readonly string Key;

        public DDialogChoice(JObject json)
        {
            Text = json.GetString("Text");
            Key = json.GetString("Key");
        }
    }

    public class DDialogSpeech
    {
        public readonly string Key;
        public readonly string Character;
        public readonly string Pivot;
        public readonly string Emotion;
        public readonly string Text;

        // 대사 분기
        public readonly string JumpKey;
        public readonly List<DDialogChoice> Choices;

        public DDialogSpeech(JObject json)
        {
            Key = json.GetString("Key") ?? string.Empty;
            Character = json.GetString("Character");
            Pivot = json.GetString("Pivot") ?? "Center";
            Emotion = json.GetString("Emotion") ?? "Idle";
            Text = json.GetString("Text") ?? string.Empty;

            JumpKey = json.GetString("JumpKey") ?? string.Empty;
            Choices = json.GetObjArray("Choices")
                .Select(x => new DDialogChoice(x))
                .ToList();
        }
    }

    public class DDialog : DEntry
    {
        public readonly DialogType Type;
        public readonly List<DDialogSpeech> Speeches;

        public DDialog(JObject json)
            : base(json)
        {
            Type = json.GetEnum<DialogType>("Type");
            Speeches = json.GetObjArray("Speeches")
                .Select(x => new DDialogSpeech(x))
                .ToList();
        }

        public DDialogSpeech FindSpeech(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return Speeches.FirstOrDefault(x => key == x.Key);
        }
    }
}