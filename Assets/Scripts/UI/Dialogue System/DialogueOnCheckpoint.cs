using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Dialogue_System
{
    public class DialogueOnCheckpoint : MonoBehaviour
    {
        [SerializeField] private GameObject iconToTrigger;
        private SOConversationData nextDialogue;
        
        private void OnEnable()
        {
            Checkpoint_System.Checkpoint.OnCheckpointReached += PrepDialogue;
            iconToTrigger.SetActive(false);
            SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnSceneChange(Scene _, Scene __)
        {
            DisableChitChat();
        }

        private void PrepDialogue(Checkpoint_System.Checkpoint checkpoint)
        {
            if (checkpoint.Conversation == null) return;
            iconToTrigger.SetActive(true);
            Controller.UIController.OnChitChat -= StartDialogue;
            Controller.UIController.OnChitChat += StartDialogue;
            nextDialogue = checkpoint.Conversation;
        }
        
        private void StartDialogue()
        {
            DialogueManager.Instance.StartDialogue(nextDialogue);
            DisableChitChat();
        }

        private void DisableChitChat()
        {
            iconToTrigger.SetActive(false);
            Controller.UIController.OnChitChat -= StartDialogue;
        }


        private void OnDisable()
        {
            Checkpoint_System.Checkpoint.OnCheckpointReached -= PrepDialogue;
        }
    }
}