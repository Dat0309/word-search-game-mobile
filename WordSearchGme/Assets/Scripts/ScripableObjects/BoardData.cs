using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class BoardData : ScriptableObject
{
    /// <summary>
    /// Lớp định nghĩa đối tượng bảng
    /// </summary>
    [System.Serializable]
    public class SearchingWord
    {
        [HideInInspector]
        public bool Found = false;
        public string Word;
        public string WordValue;
    }

    /*
    Định nghĩa các thao tác với các hàng
    - CreateRow: tạo một hàng với độ tài size và chứa các ký tự trong row[]
    - ClearRow: xoá hàng
     */
    [System.Serializable]
    public class BoardRow
    {
        public int Size;
        public string[] Row;

        public BoardRow()
        {

        }

        public BoardRow(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            this.Size = size;
            this.Row = new string[this.Size];
            ClearRow();
        }

        public void ClearRow()
        {
            for(int i=0; i < Size; i++)
            {
                Row[i] = " ";
            }
        }
    }

    public float timeInSeconds;
    public int Columns = 0;
    public int Rows = 0;

    public BoardRow[] Board;
    public List<SearchingWord> SearchWords = new List<SearchingWord>();

    public void ClearData()
    {
        foreach (var word in SearchWords)
        {
            word.Found = false;
        }
    }
    /// <summary>
    /// Xoá trắng bảng
    /// </summary>
    public void ClearWithEmptyString()
    {
        for(int i=0; i< Columns; i++)
        {
            Board[i].ClearRow();
        }
    }
    
    /// <summary>
    /// Hàm tạo bảng mới
    /// </summary>
    public void CreateNewBoard()
    {
        Board = new BoardRow[Columns];
        for(int i=0;i< Columns; i++)
        {
            Board[i] = new BoardRow(Rows);
        }
    }
}
