using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUtility : MonoBehaviour
{
    public GameObject DetailDialog;
    /// <summary>
    /// Lớp định nghĩa các hàm thao tác tổng thể với game
    /// </summary>
    /// <param name="sceneName"></param>
    /// 
    //Hàm Chuyển màn hình game
   public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    //Hàm thoát game
    public void ExitApplication()
    {
        Application.Quit();
    }

    // Hàm tắt nhạc nền
    public void MuteToggleBackgroundMusic()
    {
        SoundManager.instance.ToggleBackgroundMusic();
    }

    // Hàm tắt tiếng của trò chơi
    public void MuteToggleSoundFX()
    {
        SoundManager.instance.ToggleSoundFX();
    }

    public void ResumeBtn()
    {
        DetailDialog.SetActive(false);
    }
}
