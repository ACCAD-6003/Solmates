using System.Collections.Generic;
using Controller;
using UnityEngine;
using static UI.Dialogue_System.DialogueHelperClass;

namespace UI.Dialogue_System
{
    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] private TextBoxDisplay textBoxDisplay;
        [SerializeField] private ConversantType player;

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
            DialogueManager.OnTextUpdated -= textBoxDisplay.UpdateDialogueText;
        }

        private void DisplayUI(ConversationData conversation, ConversantType playerWhoEnteredDialogue)
        {
            if (player != playerWhoEnteredDialogue) return;
        
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        
            textBoxDisplay.Display(player);
            DialogueManager.OnTextUpdated += textBoxDisplay.UpdateDialogueText;
        }

        private void OnDisable()
        {
            DialogueManager.OnDialogueStarted -= DisplayUI;
            DialogueManager.OnDialogueEnded -= HideUI;
        }

        private void OnDestroy()
        {
            DialogueManager.OnTextUpdated -= textBoxDisplay.UpdateDialogueText;
        }
    }
}