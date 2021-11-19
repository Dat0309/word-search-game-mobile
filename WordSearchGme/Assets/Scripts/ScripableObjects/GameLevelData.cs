using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class GameLevelData : ScriptableObject
{
    /// <summary>
    /// Lớp định nghĩa các màn chơi của game
    /// </summary>
    [System.Serializable]
    public struct CategoryRecord
    {
        /// <summary>
        /// Tên màn chơi
        /// </summary>
        public string CatName;
        /// <summary>
        /// Các bảng từ ngữ của màn chơi
        /// </summary>
        public List<BoardData> boardData;
    }

    public List<CategoryRecord> data;
}
