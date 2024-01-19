using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using static DialogueHelperClass;

public class DialogueManager : SingletonMonoBehavior<DialogueManager>
{
    public static Action<ConversationData> OnDialogueStarted;
    public static Action OnDialogueEnded;
    public static Action<string, bool> OnTextUpdated;
    public static Action<List<string>> OnChoiceMenuOpen;
    public static Action OnChoiceMenuClose;

    [SerializeField] float dialogueSpeed;
    [SerializeField] float dialogueFastSpeed;
    [SerializeField, ReadOnly] List<SOConversationData> conversationGroup;

    Dictionary<string, DialogueBranchData> choiceToPath = new Dictionary<string, DialogueBranchData>();

    float currentDialogueSpeed;
    bool inDialogue;
    bool continueInputRecieved;
    bool abortDialogue;
    string choiceSelected;
    public bool InDialogue => inDialogue;
    public bool ValidateID(string id) => conversationGroup.Find(data => data.Data.ID.ToLower().Equals(id.ToLower()));

    protected override void Awake()
    {
        base.Awake();
        conversationGroup = Resources.LoadAll<SOConversationData>("Dialogue").ToList();
    }

    [Button]
    public void StartDialogue(SOConversationData conversation)
    {
        StartDialogue(conversation.Data.ID);
    }

    public void StartDialogue(string dialogueId)
    {
        if (dialogueId == null || dialogueId.ToLowerInvariant().Equals("exit"))
        {
            ExitDialogue();
            return;
        }
        else if (!inDialogue)
        {
            inDialogue = true;
            Controller.Instance.SwapToUI();
        }

        var SOConversationData = conversationGroup.Find(data => data.Data.ID.ToLower().Equals(dialogueId.ToLower()));
        if (SOConversationData == null)
        {
            Debug.LogError("Could not find " + dialogueId + " in database");
            return;
        }

        StartCoroutine(HandleConversation(SOConversationData.Data));
    }

    private void ExitDialogue()
    {
        inDialogue = false;
        OnDialogueEnded?.Invoke();
        Controller.Instance.SwapToGameplay();
    }

    private void OnAbort() 
    {
        abortDialogue = true;
        OnContinueInput();
    }

    private IEnumerator HandleConversation(ConversationData data)
    {
        OnDialogueStarted?.Invoke(data);

        if (data.Dialogues.Count >= 1 && !data.Dialogues[0].Dialogue.IsNullOrWhitespace())
        {
            abortDialogue = false;
            Controller.OnOverrideSkip += OnAbort;

            foreach (var dialogue in data.Dialogues)
            {
                yield return ProcessDialogue(dialogue, data.Conversant);
                if (abortDialogue) break;
            }

            Controller.OnOverrideSkip -= OnAbort;
            

        }

        GenerateChoiceToPath(data);
        yield return HandleChoices();
        ExitDialogue();
    }

    public void SelectChoice(string choice) => choiceSelected = choice;

    private IEnumerator HandleChoices()
    {
        choiceSelected = null;
        if (choiceToPath.Count == 0) yield break;

        OnChoiceMenuOpen?.Invoke(choiceToPath.Keys.ToList());
        yield return new WaitUntil(() => choiceSelected != null);
        OnChoiceMenuClose?.Invoke();
    }

    private void GenerateChoiceToPath(ConversationData conversation)
    {
        choiceToPath.Clear();
        for (int i = 0; i < conversation.Choices.Count; i++)
        {
            choiceToPath.Add(conversation.Choices[i], conversation.LeadsTo[i]);
        }
    }

    private void OnContinueInput() => continueInputRecieved = true;

    private IEnumerator ProcessDialogue(DialogueData dialogue, string conversant)
    {
        OnTextUpdated?.Invoke("", dialogue.PlayerIsSpeaker);
        yield return new WaitUntil(() => FadeToBlackSystem.FadeOutComplete);

        continueInputRecieved = false;
        string name = "";

        if (!dialogue.VoiceSpeaker)
        {
            name =  "<u>" + (dialogue.PlayerIsSpeaker ? PLAYER_MARKER : (conversant + ": ")) + "</u>\n";
        }

        yield return TypewriterDialogue(name, dialogue.Dialogue, dialogue.PlayerIsSpeaker);

        Controller.OnNextDialogue += OnContinueInput;

        yield return new WaitUntil(() => continueInputRecieved);

        Controller.OnNextDialogue -= OnContinueInput;
    }

    private IEnumerator TypewriterDialogue(string name, string line, bool isWickSpeaker)
    {
        currentDialogueSpeed = dialogueSpeed;
        string loadedText = name;
        Controller.OnNextDialogue += SpeedUpText;
        bool atSpecialCharacter = false;
        foreach(char letter in line)
        {
            loadedText += letter;
            atSpecialCharacter = letter == '<' || atSpecialCharacter;
            if (atSpecialCharacter && letter != '>') continue;
            atSpecialCharacter = false;
            OnTextUpdated?.Invoke(loadedText, isWickSpeaker);
            yield return new WaitForSeconds(1 / currentDialogueSpeed);
            if (abortDialogue) { OnTextUpdated?.Invoke(name + line, isWickSpeaker); break; }
        }
        Controller.OnNextDialogue -= SpeedUpText;
    }

    private void SpeedUpText() => currentDialogueSpeed = currentDialogueSpeed == dialogueFastSpeed ? currentDialogueSpeed = dialogueFastSpeed * 10 : dialogueFastSpeed;
}
