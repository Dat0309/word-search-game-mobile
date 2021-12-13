using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    /// <summary>
    /// Lớp hiển thị màn hình Gameover
    /// </summary>

    public GameObject gameOverPopup;
    //public GameObject playAgain;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPopup.SetActive(false);

    }
    private void OnEnable()
    {
        GameEvents.OnGameOver += ShowGameOverPopup;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverPopup;
    }

    private void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);
        //playAgain.GetComponent<Button>().interactable=false;
        SoundManager.instance.PlayLoseSound();
    }

    public void LoadAgain()
    {
        GameEvents.LoadNextLevelMethod();
    }
}
