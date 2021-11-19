using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectPuzzleButton : MonoBehaviour
{
    /// <summary>
    /// Lớp định nghĩa thao tác chọn các màn chơi
    /// </summary>
    /// 

    public GameData gameData;
    /// <summary>
    /// Các màn chơi của game
    /// </summary>
    public GameLevelData levelData;
    /// <summary>
    /// Tên màn chơi
    /// </summary>
    public Text categoryText;
    /// <summary>
    /// Thanh cập nhật tiến độ chơi
    /// </summary>
    public Image progresBarFilling;

    private string gameSceneName = "GameScene";

    /// <summary>
    /// Lưu level
    /// </summary>
    private bool _levelLocked;

    // Start is called before the first frame update
    void Start()
    {
        _levelLocked = false;
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        UpdateButtonInfomation();

        if (_levelLocked)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;

        }

    }

    /// <summary>
    /// Sự kiện nhấn vào màn chơi sẽ chuyển màn hình
    /// </summary>
    private void OnButtonClick()
    {
        gameData.selectedCategoryName = gameObject.name;
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Hàm cập nhật trạng thái của màn chơi:
    /// tiến độ : curentIndex / totalBoard (10/10) 
    /// ProgresBar filling
    /// </summary>
    private void UpdateButtonInfomation()
    {
        var currentIndex = -1;
        var totalBoards = 0;

        foreach (var data in levelData.data)
        {
            if(data.CatName == gameObject.name)
            {
                currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                totalBoards = data.boardData.Count;

                if(levelData.data[0].CatName== gameObject.name &&
                    currentIndex < 0)
                {
                    DataSaver.SaveCategoryData(levelData.data[0].CatName, 0);
                    currentIndex = DataSaver.ReadCategoryCurrentIndexValues(gameObject.name);
                    totalBoards = data.boardData.Count;
                }
            }
        }

        if (currentIndex == -1)
            _levelLocked = true;

        categoryText.text = _levelLocked ? String.Empty : (currentIndex.ToString() + "/" + totalBoards.ToString());
        progresBarFilling.fillAmount = (currentIndex > 0 && totalBoards > 0) ? ((float)currentIndex / (float)totalBoards) : 0;
        

    }
}
