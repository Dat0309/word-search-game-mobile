using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    /// <summary>
    /// Hàm trả về số màn chơi đã vượt qua
    /// </summary>
    /// <param name="name"> </param>
    /// <returns></returns>
    public static int ReadCategoryCurrentIndexValues(string name)
    {
        var value = -1;
        // PlayerPrefs cho phép lưu trữ thông tin của mỗi màn chơi khác nhau ở dạng Key : value 
        if (PlayerPrefs.HasKey(name))
        {
            value = PlayerPrefs.GetInt(name);
        }

        return value;
    }

    /// <summary>
    /// Hàm lưu dữ liệu game
    /// </summary>
    /// <param name="categoryName"></param>
    /// <param name="currentIndex"></param>
    public static void SaveCategoryData(string categoryName, int currentIndex)
    {
        PlayerPrefs.SetInt(categoryName, currentIndex);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Hàm xoá dữ liệu game
    /// </summary>
    /// <param name="levelData"></param>
    public static void ClearGameData(GameLevelData levelData)
    {
        foreach (var data in levelData.data)
        {
            PlayerPrefs.SetInt(data.CatName, -1);
        }

        //Unlock first level
        PlayerPrefs.SetInt(levelData.data[0].CatName, 0);
        PlayerPrefs.Save();
    }
}
