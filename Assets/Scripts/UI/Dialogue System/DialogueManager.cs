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
    public static Action<string> OnTextUpdated;
    public static Action<List<string>> OnChoiceMenuOpen;
    public static Action OnChoiceMenuClose;

    [SerializeField, Tooltip("Chars/Second")] float dialogueSpeed;
    [SerializeField, Tooltip("Chars/Second")] float dialogueFastSpeed;
    [SerializeField, ReadOnly] List<SOConversationData> conversationGroup;
    
    private ConversantType currentPlayer;

    private Dictionary<ConversationData, int> conversationProgress = new();
    private float currentDialogueSpeed;
    private bool inDialogue;
    private bool continueInputReceived;
    private bool abortDialogue;
    private string choiceSelected;
    public bool InDialogue => inDialogue;
    public bool ValidateID(string id) => conversationGroup.Find(data => data.Data.ID.ToLower().Equals(id.ToLower()));
    

    protected override void Awake()
    {
        base.Awake();
        conversationGroup = Resources.LoadAll<SOConversationData>("Dialogue").ToList();
    }

    [Button(ButtonStyle.Box)]
    public void StartPlayerOneDialogue(SOConversationData conversation)
    {
        currentPlayer = ConversantType.PlayerOne;
        StartDialogue(conversation.Data.ID);
    }

    [Button(ButtonStyle.Box)]
    public void StartPlayerTwoDialogue(SOConversationData conversation)
    {
        currentPlayer = ConversantType.PlayerTwo;
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

        var ConversationDataPointer = conversationGroup.Find(data => data.Data.ID.ToLower().Equals(dialogueId.ToLower()));
        if (ConversationDataPointer == null)
        {
            Debug.LogError("Could not find " + dialogueId + " in database");
            return;
        }
        if (conversationProgress.ContainsKey(ConversationDataPointer.Data))
        {
            conversationProgress[ConversationDataPointer.Data]++;
        }
        else
        {
            conversationProgress.Add(ConversationDataPointer.Data, 0);
        }

        StartCoroutine(HandleConversation(ConversationDataPointer.Data));
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
        var dialogueChain = conversationProgress[data] < data.DialoguesSeries.Count ? 
            data.DialoguesSeries[conversationProgress[data]] : data.DialoguesSeries[^1];

        if (dialogueChain.dialogues.Count >= 1 && !dialogueChain.dialogues[0].Dialogue.IsNullOrWhitespace())
        {
            abortDialogue = false;
            Controller.OnOverrideSkip += OnAbort;

            foreach (var dialogue in dialogueChain.dialogues)
            {
                switch (currentPlayer)
                {
                    case ConversantType.PlayerOne when dialogue.speaker == ConversantType.PlayerTwo:
                    case ConversantType.PlayerTwo when dialogue.speaker == ConversantType.PlayerOne:
                        continue;
                }
                yield return ProcessDialogue(dialogue, data.Conversant);
                if (abortDialogue) break;
            }

            Controller.OnOverrideSkip -= OnAbort;
        }

        yield return HandleChoices(dialogueChain.choices);
        ExitDialogue();
    }

    public void SelectChoice(string choice) => choiceSelected = choice;

    private IEnumerator HandleChoices(List<string> choices)
    {
        choiceSelected = null;
        if (choices.Count == 0) yield break;

        OnChoiceMenuOpen?.Invoke(choices);
        yield return new WaitUntil(() => choiceSelected != null);
        OnChoiceMenuClose?.Invoke();
    }

    private void OnContinueInput() => continueInputReceived = true;

    private IEnumerator ProcessDialogue(DialogueData dialogue, string conversant)
    {
        string Underline(string text) => "<u>" + text + "</u>";
        OnTextUpdated?.Invoke("");
        yield return new WaitUntil(() => FadeToBlackSystem.FadeOutComplete);

        continueInputReceived = false;
        var speakerName = "";

        if (dialogue.speaker != ConversantType.Other)
        {
            speakerName = dialogue.speaker switch
            {
                ConversantType.PlayerOne => PLAYER_MARKER,
                ConversantType.PlayerTwo => PLAYER_TWO_MARKER,
                ConversantType.Conversant => conversant,
                _ => speakerName
            };
            speakerName = Underline(speakerName) + "\n";
        }

        yield return TypewriterDialogue(speakerName, dialogue.Dialogue);

        Controller.OnNextDialogue += OnContinueInput;

        yield return new WaitUntil(() => continueInputReceived);

        Controller.OnNextDialogue -= OnContinueInput;
    }

    private IEnumerator TypewriterDialogue(string name, string line)
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
            OnTextUpdated?.Invoke(loadedText);
            yield return new WaitForSeconds(1 / currentDialogueSpeed);
            if (abortDialogue) { OnTextUpdated?.Invoke(name + line); break; }
        }
        Controller.OnNextDialogue -= SpeedUpText;
    }

    private void SpeedUpText() => currentDialogueSpeed = currentDialogueSpeed == dialogueFastSpeed ? currentDialogueSpeed = dialogueFastSpeed * 10 : dialogueFastSpeed;
}
