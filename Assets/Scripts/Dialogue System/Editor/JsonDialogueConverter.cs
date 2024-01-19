using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.Utilities;
using UnityEditor;
using static DialogueHelperClass;

public static class JsonDialogueConverter
{ 
    public static string ConvertToJson(ConversationData conversation)
    {
        return JsonUtility.ToJson(conversation, true);
    }

    public static void ConvertToJson(string text)
    {
        foreach (string dialogueScene in text.Split(ID_MARKER, StringSplitOptions.RemoveEmptyEntries)) {
            Debug.Log(dialogueScene);
            SOConversationData conversation = ScriptableObject.CreateInstance<SOConversationData>();
            conversation.SetConversation(ConvertFromJson(ConvertToJson(ConvertToConversation(dialogueScene))));

            string filePath = $"Assets/Resources/Dialogue/{conversation.Data.ID}.asset";
            if (System.IO.File.Exists(filePath))
            {
                var file = AssetDatabase.LoadAssetAtPath(filePath, typeof(SOConversationData)) as SOConversationData;
                file.SetConversation(conversation.Data);
                EditorUtility.SetDirty(file);
            }
            else
            {
                AssetDatabase.CreateAsset(conversation, filePath);
            }
        }
    }

    public static ConversationData ConvertFromJson(string jsonFile)
    {
        return JsonUtility.FromJson<ConversationData>(jsonFile);
    }

    public static ConversationData ConvertFromJson(TextAsset jsonFile)
    {
        return ConvertFromJson(jsonFile.text);
    }

    private static ConversationData ConvertToConversation(string text)
    {
        var conversation = new ConversationData();
        var lines = text.Split('\n').Where(x => !x.IsNullOrWhitespace()).Select(x => x.Trim()).ToList();

        conversation.ID = lines[0];
        Debug.Log($"Converting {lines[0]}");
        lines.RemoveAt(0);


        AssertMarker(lines[0], CONVERSANT_MARKER);
        conversation.Conversant = lines[0].Substring(CONVERSANT_MARKER.Length);
        lines.RemoveAt(0);

        while (lines.Count > 0)
        {
            AssertMarker(lines[0], DIALOGUE_MARKER);
            lines.RemoveAt(0);
            
            conversation.DialoguesSeries.Add(new List<DialogueData>());

            while (!lines[0].StartsWith(CHOICES_MARKER))
            {
                if (!lines[0].StartsWith(PLAYER_MARKER) && !lines[0].StartsWith($"{conversation.Conversant}: ") &&
                    !lines[0].StartsWith(VOICE_MARKER))
                {
                    conversation.DialoguesSeries[^1][^1].Dialogue += " " + lines[0];
                }
                else
                {
                    string dialogueLine = "";
                    if (lines[0].StartsWith(PLAYER_MARKER))
                    {
                        dialogueLine = lines[0][PLAYER_MARKER.Length..];
                    }
                    else if (lines[0].StartsWith(PLAYER_TWO_MARKER))
                    {
                        dialogueLine = lines[0][PLAYER_TWO_MARKER.Length..];
                    }
                    else if (lines[0].StartsWith(VOICE_MARKER))
                    {
                        dialogueLine = lines[0][VOICE_MARKER.Length..];
                    }
                    else
                    {
                        dialogueLine = lines[0][(conversation.Conversant.Length + ": ".Length)..];
                    }

                    conversation.DialoguesSeries[^1].Add(new DialogueData()
                    {
                        Dialogue = dialogueLine.Trim(), VoiceSpeaker = lines[0].StartsWith(VOICE_MARKER),
                        PlayerIsSpeaker = lines[0].StartsWith(PLAYER_MARKER)
                    });
                }

                lines.RemoveAt(0);
            }
            
            lines.RemoveAt(0);
            conversation.ChoiceSeries.Add(new List<string>());
            
            while (lines.Count > 0 && !lines[0].StartsWith(DIALOGUE_MARKER))
            {
                conversation.ChoiceSeries[^1].Add(lines[0]);
                lines.RemoveAt(0);
            }
        }

        return conversation;
    }

    private static List<RequirementData> GenerateRequirmentsData(string[] branchLines)
    {
        var requirments = new List<RequirementData>();

        for (int i = 1; i < branchLines.Length; i++)
        {
            var requirment = new RequirementData();
            int offset = 0;

            if (branchLines[i][0] == '$')
            {
                offset++;
                requirment.isItemID = true;
            }
            if (branchLines[i].Length > 1 && branchLines[i][1] == '$')
            {
                offset++;
                requirment.consumesItem = true;
            }

            requirment.label = branchLines[i][offset..].ToLowerInvariant();

            requirments.Add(requirment);
        }

        return requirments;
    }

    private static bool AssertMarker(string text, string marker)
    {
        Debug.Assert(text.StartsWith(marker), $"ERROR: {text} did not start with {marker}");
        return text.StartsWith(marker);
    }
}
