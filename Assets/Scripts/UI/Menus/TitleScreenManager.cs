using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] ButtonGroup menuButtons;
    [SerializeField] int firstLevelIndex;

    private void Start()
    {
        Controller.Instance.SwapToUI();
        menuButtons.EnableButtons();
    }

    public void StartGame()
    {
        Controller.Instance.SwapToGameplay();
        StartCoroutine(SceneTools.TransitionToScene(firstLevelIndex));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
