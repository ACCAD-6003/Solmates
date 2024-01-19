using UnityEngine;
using static DialogueHelperClass;

[RequireComponent(typeof(AudioSource))]
public class DialogueSoundListener : MonoBehaviour
{
    [SerializeField] DialogueSoundDatabase soundDatabase;
    new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        DialogueManager.OnDialogueStarted += TriggerAudio;
    }

    private void TriggerAudio(ConversationData dialogueNode)
    {
        if (soundDatabase.GetClip(dialogueNode.Sound, out var clip))
        {
            audio.PlayOneShot(clip);
        }
    }

    private void OnDestroy()
    {
        DialogueManager.OnDialogueStarted -= TriggerAudio;
    }
}
