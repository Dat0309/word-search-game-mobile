using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    /// <summary>
    /// Lớp hiển thị màn hình Gameover
    /// </summary>

    public GameObject gameOverPopup;
    public GameObject continueGameAfterAdsButton;

    // Start is called before the first frame update
    void Start()
    {
        continueGameAfterAdsButton.GetComponent<Button>().interactable = false;
        gameOverPopup.SetActive(false);

        GameEvents.OnGameOver += ShowGameOverPopup;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ShowGameOverPopup;
    }

    private void ShowGameOverPopup()
    {
        gameOverPopup.SetActive(true);
        continueGameAfterAdsButton.GetComponent<Button>().interactable=false;
        SoundManager.instance.PlayLoseSound();
    }
}
