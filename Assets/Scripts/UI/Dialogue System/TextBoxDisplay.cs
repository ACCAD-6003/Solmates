using TMPro;
using UnityEngine;

namespace UI.Dialogue_System
{
    public class TextBoxDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI dialogueTextField;

        private DialogueHelperClass.ConversantType conversant;

        public void Display(DialogueHelperClass.ConversantType conversantToShow)
        {
            ToggleChildrenDisplay(true);
            conversant = conversantToShow;
        }

        public void UpdateDialogueText(string text, DialogueHelperClass.ConversantType playerListening)
        {
            if (playerListening != conversant) return;
            dialogueTextField.text = text;
        }

        public void Hide() => ToggleChildrenDisplay(false);

        private void ToggleChildrenDisplay(bool shouldDisplay)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldDisplay);
            }
        }
    }
}