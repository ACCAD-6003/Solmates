using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using static UI.Dialogue_System.DialogueHelperClass;

namespace UI.Dialogue_System
{
    public class DialogueUIController : SerializedMonoBehaviour
    {
        [SerializeField] private TextBoxDisplay textBoxDisplay;
        [SerializeField] private ConversantType player;
        [SerializeField] private Dictionary<ConversantType, Sprite> dialogueBackgrounds;
        [SerializeField] private Image dialogueBackground;

        private void OnEnable()
        {
            DialogueManager.OnDialogueStarted += DisplayUI;
            DialogueManager.OnDialogueEnded += HideUI;
            HideUI();
        }

        private void HideUI()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        
            textBoxDisplay.Hide();
            DialogueManager.OnTextUpdated -= UpdateDialogue;
        }

        private void DisplayUI(ConversationData conversation, ConversantType playerWhoEnteredDialogue)
        {
            if (player != playerWhoEnteredDialogue) return;
        
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        
            textBoxDisplay.Display(player);
            DialogueManager.OnTextUpdated += UpdateDialogue;
        }

        private void UpdateDialogue(string text, ConversantType playerListener, ConversantType speaker)
        {
            textBoxDisplay.UpdateDialogueText(text, playerListener);
            dialogueBackground.sprite = dialogueBackgrounds[speaker];
        }

        private void OnDisable()
        {
            DialogueManager.OnDialogueStarted -= DisplayUI;
            DialogueManager.OnDialogueEnded -= HideUI;
        }

        private void OnDestroy()
        {
            DialogueManager.OnTextUpdated -= UpdateDialogue;
        }
    }
}