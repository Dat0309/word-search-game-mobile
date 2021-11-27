using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPopup : MonoBehaviour
{
    /// <summary>
    /// Lớp xử lý sự kiện khi người chơi thắng màn chơi
    /// </summary>

    public GameObject winPopup;
    // Start is called before the first frame update
    void Start()
    {
        winPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += ShowWinPopup;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= ShowWinPopup;
        
    }
    /// <summary>
    /// Hàm hiện màn hình thông báo thắng
    /// </summary>
    private void ShowWinPopup()
    {
        winPopup.SetActive(true);
        SoundManager.instance.PlayWinSound();
    }

    public void LoadNextLevel()
    {
        GameEvents.LoadNextLevelMethod();
    }
}
