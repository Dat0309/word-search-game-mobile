using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSelector : MonoBehaviour
{
    /// <summary>
    /// Lớp định nghĩa các màn chơi
    /// </summary>
    /// 

    public GameData currentGameData;
    public GameLevelData LevelData;

    // Start is called before the first frame update
    void Awake()
    {
        SelectSequentalBoardData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Hàm chọn màn chơi
    /// Sẽ chọn ra các bảng các chữ cái của màn chơi đó
    /// </summary>
    private void SelectSequentalBoardData()
    {
        foreach (var data in LevelData.data)
        {
            if(data.CatName == currentGameData.selectedCategoryName)
            {
                // Lấy màn chơi hiện tại
                var boardIndex = DataSaver.ReadCategoryCurrentIndexValues(currentGameData.selectedCategoryName);
                if(boardIndex < data.boardData.Count)
                {
                    currentGameData.selectedBoardData = data.boardData[boardIndex];
                }
                else
                {
                    var randomIndex = Random.Range(0, data.boardData.Count);
                    currentGameData.selectedBoardData = data.boardData[randomIndex];
                }
            }
        }
    }
}
