using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Sirenix.OdinInspector;
using UnityEngine;
using static UI.Dialogue_System.DialogueHelperClass;

namespace UI.Dialogue_System
{
    public class DialogueManager : SingletonMonoBehavior<DialogueManager>
    {
        public static Action<ConversationData, ConversantType> OnDialogueStarted;
        public static Action OnDialogueEnded;
        public static Action<string, ConversantType, ConversantType> OnTextUpdated;

        [SerializeField, Tooltip("Chars/Second")] float dialogueSpeed;
        [SerializeField, Tooltip("Chars/Second")] float dialogueFastSpeed;
        [SerializeField, ReadOnly] List<SOConversationData> conversationGroup;

        private readonly Dictionary<string, int> dialogueProgress = new();
    
        private float currentDialogueSpeed;
        private bool inDialogue;
        private bool continueInputReceived;
        private bool abortDialogue;
        public bool InDialogue => inDialogue;
        public bool ValidateID(string id) => conversationGroup.Find(data => data.Data.ID.ToLower().Equals(id.ToLower()));
        private int playersReady;
    

        protected override void Awake()
        {
            base.Awake();
            conversationGroup = Resources.LoadAll<SOConversationData>("Dialogue").ToList();
        }

        [Button(ButtonStyle.Box)]
        public void StartDialogue(SOConversationData conversation)
        {
            if (inDialogue) return;
            
            AdvanceDialogue(conversation.Data.ID);
            StartDialogue(conversation.Data.ID, ConversantType.PlayerOne);
            StartDialogue(conversation.Data.ID, ConversantType.PlayerTwo);
        }
    
        private void AdvanceDialogue(string data)
        { 
            if (dialogueProgress.ContainsKey(data))
            {
                dialogueProgress[data]++;
            }
            else
            {
                dialogueProgress.Add(data, 0);
            }
        }

        public void StartDialogue(string dialogueId, ConversantType player)
        {
            if (dialogueId == null || dialogueId.ToLowerInvariant().Equals("exit"))
            {
                ExitDialogue();
                return;
            }
            
            if (!inDialogue)
            {
                inDialogue = true;
                UIController.Instance.SwapToUI();
            }

            var ConversationDataPointer = conversationGroup.Find(data => data.Data.ID.ToLower().Equals(dialogueId.ToLower()));
            if (ConversationDataPointer == null)
            {
                Debug.LogError("Could not find " + dialogueId + " in database");
                return;
            }

            StartCoroutine(HandleConversation(ConversationDataPointer.Data, player));
        }

        private void ExitDialogue()
        {
            inDialogue = false;
            OnDialogueEnded?.Invoke();
            UIController.Instance.SwapToGameplay();
        }

        private void OnAbort() 
        {
            abortDialogue = true;
            OnContinueInput();
        }

        private IEnumerator HandleConversation(ConversationData data, ConversantType player)
        {
            OnDialogueStarted?.Invoke(data, player);

            abortDialogue = false;
            UIController.OnOverrideSkip += OnAbort;
        
            var dialogueIndex = dialogueProgress.TryGetValue(data.ID, out var progress)
                ? Mathf.Min(progress, data.DialoguesSeries.Count - 1)
                : 0;
            var dialogues = data.DialoguesSeries[dialogueIndex].dialogues;
        

            foreach (var dialogue in dialogues)
            {
                if (data.Conversant != PLAYER_SPEAKING_TO_EACH_OTHER_LABEL)
                {
                    switch (player)
                    {
                        case ConversantType.PlayerOne when dialogue.speaker == ConversantType.PlayerTwo:
                        case ConversantType.PlayerTwo when dialogue.speaker == ConversantType.PlayerOne:
                            continue;
                    }
                }
                playersReady = 0;

                yield return ProcessDialogue(dialogue, player, data.Conversant);
                if (abortDialogue) break;
            }

            UIController.OnOverrideSkip -= OnAbort;

            var nextDialogueIndex = dialogueProgress.TryGetValue(data.ID, out var p)
                ? Mathf.Min(p, data.LeadsTo.Count - 1)
                : 0;
            var nextDialogue = data.LeadsTo[nextDialogueIndex];
            
            if(nextDialogue.ToLower().StartsWith("end"))
                ExitDialogue();
            else
                StartDialogue(nextDialogue, player);
        }
    
        private void OnContinueInput() => continueInputReceived = true;

        private IEnumerator ProcessDialogue(DialogueData dialogue, ConversantType player, string conversant)
        {
            OnTextUpdated?.Invoke("", player, dialogue.speaker);
            yield return new WaitUntil(() => FadeToBlackSystem.FadeOutComplete);

            continueInputReceived = false;
            var speakerName = "";

            speakerName = SpeakerName(dialogue, conversant);

            yield return TypewriterDialogue(speakerName, player, dialogue);
            playersReady++;

            if(playersReady == 2) UIController.OnNextDialogue += OnContinueInput;

            yield return new WaitUntil(() => continueInputReceived);

            UIController.OnNextDialogue -= OnContinueInput;
        }

        private static string SpeakerName(DialogueData dialogue, string conversant)
        {
            var speakerName = "";
            if (dialogue.speaker == ConversantType.Other) return speakerName;
            
            speakerName = dialogue.speaker switch
            {
                ConversantType.PlayerOne => PLAYER_MARKER,
                ConversantType.PlayerTwo => PLAYER_TWO_MARKER,
                ConversantType.Conversant => conversant + ": ",
                _ => speakerName
            };
            speakerName = Underline(speakerName) + "\n";

            return speakerName;

            string Underline(string text) => "<u>" + text + "</u>";
        }

        private IEnumerator TypewriterDialogue(string name, ConversantType player, DialogueData dialogue)
        {
            currentDialogueSpeed = dialogueSpeed;
            var loadedText = name;
            UIController.OnNextDialogue += SpeedUpText;
            var atSpecialCharacter = false;
            var charsInRow = 0;
            var line = dialogue.Dialogue;

            for (var index = 0; index < line.Length; index++)
            {
                var letter = line[index];
                var nextSpace = line.IndexOf(' ', index);
                var willClipLine = (nextSpace != -1 && charsInRow + (nextSpace - index) > 50) || (charsInRow > 50 && letter == ' ');
                if (willClipLine)
                {
                    loadedText += "\n";
                    charsInRow = 0;
                    if (letter == ' ')
                    {
                        continue;
                    }
                }
                else
                {
                    charsInRow++;
                }
            
                loadedText += letter;
                atSpecialCharacter = letter == '<' || atSpecialCharacter;
                if (atSpecialCharacter && letter != '>') continue;
                atSpecialCharacter = false;
                OnTextUpdated?.Invoke(loadedText, player, dialogue.speaker);
                yield return new WaitForSeconds(1 / currentDialogueSpeed);
                if (abortDialogue)
                {
                    OnTextUpdated?.Invoke(name + line, player, dialogue.speaker);
                    break;
                }
            }

            UIController.OnNextDialogue -= SpeedUpText;
        }

        private void SpeedUpText() => currentDialogueSpeed = currentDialogueSpeed == dialogueFastSpeed ? currentDialogueSpeed = dialogueFastSpeed * 10 : dialogueFastSpeed;
    }
}
