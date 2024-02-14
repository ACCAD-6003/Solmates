using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI.Dialogue_System.DialogueHelperClass;

namespace UI.Dialogue_System
{
    public class DialogueUIController : SerializedMonoBehaviour
    {
        [SerializeField] private TextBoxDisplay textBoxDisplay;
        [SerializeField] private ConversantType player;
        [SerializeField] private Dictionary<ConversantType, DialogueDisplay> dialogueBackgrounds;

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
            dialogueBackgrounds.Values.Select(x => x.background).ForEach(x => x.SetActive(false));
            dialogueBackgrounds[speaker].background.SetActive(true);
            textBoxDisplay.SwapDialogueTextField(dialogueBackgrounds[speaker].textField);
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

        [System.Serializable]
        public class DialogueDisplay
        {
            public TMP_Text textField;
            public GameObject background;
        }
    }
}