using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DialogueHelperClass
{
    public static readonly string ID_MARKER = "ID: ";
    public static readonly string CONVERSANT_MARKER = "Conversant: ";
    public static readonly string UNLOCKS_MARKER = "Unlocks: ";
    public static readonly string DIALOGUE_MARKER = "Dialogue:";
    public static readonly string PLAYER_MARKER = "Centa: ";
    public static readonly string PLAYER_TWO_MARKER = "Uri: ";
    public static readonly string VOICE_MARKER = "Voice: ";
    public static readonly string CHOICES_MARKER = "Choices:";
    public static readonly string LEADS_TO_MARKER = "Leads to:";
    public static readonly string SOUND_MARKER = "Music:";
    public static readonly string EMPTY_MARKER = "N/A";
    public static readonly string POTION_MADE_UNLOCK = "ITEMCRAFTED";
    public static readonly string SUCCESS = "SUCCESS";
    public static readonly string FAILURE = "FAIL";
    public static readonly List<string> PREFIXES = new List<string>() { "F", "B", "A" };

    public static List<string> POTION_GIVEN_MARKERS => PREFIXES.Select(p => p + SUCCESS).Concat(PREFIXES.Select(p => p + FAILURE)).Select(p => p.ToLower()).ToList();


    [System.Serializable]
    public class DialogueData
    {
        public bool PlayerIsSpeaker;
        public bool VoiceSpeaker;
        [SerializeField, TextArea()] public string Dialogue;
    }

    [System.Serializable]
    public class ConversationData
    {
        public string ID;
        public string Conversant;
        public string Unlocks;
        public string Sound;
        public List<List<DialogueData>> DialoguesSeries = new List<List<DialogueData>>();
        public List<DialogueData> Dialogues = new List<DialogueData>();
        public List<List<string>> ChoiceSeries = new List<List<string>>();
        public List<string> Choices = new List<string>();
        public List<DialogueBranchData> LeadsTo = new List<DialogueBranchData>();
    }

    [System.Serializable]
    public class DialogueBranchData
    {
        public string BranchText;
        public bool isPuzzle;
        public List<RequirementData> Requirements;
    }

    [System.Serializable]
    public class RequirementData
    {
        public string label;
        public bool isItemID;
        [ShowIf("isItemID", true)] public bool consumesItem;
    }
}