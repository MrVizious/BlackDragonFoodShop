using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SceneController : Singleton<SceneController>
{

    private string nextScene;

    [Button]
    public void GoToMainMenu()
    {
        nextScene = "MainMenu";
        Animator transitionAnimator = GetComponent<Animator>();
        transitionAnimator.Play("SceneOutTransition");
    }

    [Button]
    public void GoToGame()
    {
        nextScene = "MainScene";
        Animator transitionAnimator = GetComponent<Animator>();
        transitionAnimator.Play("SceneOutTransition");
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

}