﻿using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.Events;
using static DialogueHelperClass;

public class DialogueEvents : MonoBehaviour
{
    [SerializeField] GameObject imageToDisplay;
    [SerializeField] SOConversationData imageConversation;
    [SerializeField] AudioSource audioToPlayFrom;
    [SerializeField] SOConversationData audioConversation;

    bool triggerImage;
    bool triggerAudio;
    bool awaitingInput;

    private void OnEnable()
    {
        DialogueManager.OnDialogueStarted += UpdateLastDialogue;
        //DialogueManager.OnDialogueEnded += CheckForEvent;
        DialogueManager.OnChoiceMenuClose += CheckForEvent;
    }

    private void UpdateLastDialogue(ConversationData obj)
    {
        triggerImage = imageConversation.Data.Equals(obj);
        triggerAudio = audioConversation.Data.Equals(obj);
    }

    private void CheckForEvent()
    {
        if (triggerImage && imageToDisplay != null)
        {
            StartCoroutine(PlayImage());
        }
        if (triggerAudio && audioToPlayFrom != null)
        {
            audioToPlayFrom.enabled = false;
        }
    }

    private IEnumerator PlayImage()
    {
        imageToDisplay.SetActive(true);

        yield return new WaitUntil(() => UIController.Instance.InGameplay);
        UIController.Instance.SwapToUI();
        UIController.OnSelect += AcceptInput;
        awaitingInput = true;
        yield return new WaitUntil(() => !awaitingInput);
        UIController.OnSelect -= AcceptInput;
        UIController.Instance.SwapToGameplay();

        imageToDisplay.SetActive(false);
    }

    private void AcceptInput() => awaitingInput = false;

    private void OnDisable()
    {
        DialogueManager.OnDialogueStarted -= UpdateLastDialogue;
        DialogueManager.OnChoiceMenuClose -= CheckForEvent;
    }
}