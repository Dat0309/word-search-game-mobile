using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTime : MonoBehaviour
{
    /// <summary>
    /// Lớp thiết lập logic đếm ngược thời gian cho trò chơi
    /// </summary>

    public GameData currentGameData;
    public Text timerText;
    public static CountDownTime instance;
    private float _timeLeft;
    private float _minutes;
    private float _seconds;
    private float _oneSecondDown;
    private bool _timeOut;
    private bool _stopTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _stopTimer = false;
        _timeOut = false;
        _timeLeft = currentGameData.selectedBoardData.timeInSeconds;
        _oneSecondDown = _timeLeft - 1f;

        GameEvents.OnDetailWord += StopTimer;
        GameEvents.OnBoardCompleted += StopTimer;
        GameEvents.OnUnlockNextCategory += StopTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnDetailWord -= StopTimer;
        GameEvents.OnBoardCompleted -= StopTimer;
        GameEvents.OnUnlockNextCategory -= StopTimer;
    }

    public void StopTimer()
    {
        _stopTimer=true;
    }

    public void ResumTimer()
    {
        _stopTimer = false;
    }

    private void Update()
    {
        if (!_stopTimer)
        {
            _timeLeft -= Time.deltaTime;
        }

        if (_timeLeft <= _oneSecondDown)
        {
            _oneSecondDown = _timeLeft - 1f;
        }
    }

    /// <summary>
    /// Thiết lập thời gian đếm ngược cho game
    /// </summary>
    private void OnGUI()
    {
        if(_timeOut == false)
        {
            if(_timeLeft > 0)
            {
                _minutes = Mathf.Floor(_timeLeft / 60);
                _seconds = Mathf.RoundToInt(_timeLeft % 60);

                timerText.text = _minutes.ToString("00") + ":" + _seconds.ToString("00");
            }
            else
            {
                _stopTimer = true;
                ActivateGameOverGUI();
            }
        }
    }

    private void ActivateGameOverGUI()
    {
        GameEvents.GameOverMethod();
        _timeOut = true;
    }
}
