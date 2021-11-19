using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    public delegate void EnabelSquareSelection();
    public static event EnabelSquareSelection OnEnableSquareSelection;

    public static void EnableSquareSelectionMethod()
    {
        // kiểm tra xem lựa chọn hình vuông có bật không
        if (OnEnableSquareSelection != null)
        {
            OnEnableSquareSelection();
        }
    }

    //*******************************************
    public delegate void DisableSquareSelection();
    public static event DisableSquareSelection OnDisableSquareSelection;

    public static void DisableSquareSelectionMethod()
    {
        
        if (OnDisableSquareSelection != null)
        {
            OnDisableSquareSelection();
        }
    }

    /// <summary>
    /// CHU THICH 
    /// </summary>
    /// <param name="position"></param>
    /// 
    public delegate void SelectSquare(Vector3 position);
    public static event SelectSquare OnSelectSquare;

    public static void SelectSquareMethod(Vector3 positon)
    {

        if (OnSelectSquare != null)
        {
            OnSelectSquare(positon);
        }
    }


    //*******************************************
    //sự kiện kiểm tra hình ô vuông đã chọn, mục đích để kiểm tra kết quả đúng hay sai
    public delegate void CheckSquare(string letter, Vector3 squarePosition, int squareIndex);
    public static event CheckSquare OnCheckSquare;

    public static void CheckSquareMethod(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (OnCheckSquare != null)
        {
            OnCheckSquare(letter, squarePosition, squareIndex);
        }
    }

    //*******************************************
    // người dùng có thể xóa các mục chọn khi thả chuột ra
    public delegate void ClearSelection();
    public static event ClearSelection OnClearSelection;

    public static void ClearSelectionMethod()
    {
        if (OnClearSelection != null)
        {
            OnClearSelection();
        }
    }
    /// <summary>
    /// Định nghĩa logic so sánh từ ngữ
    /// </summary>
    /// <param name="word">Từ ngữ so sánh</param>
    /// <param name="squareIndex">vị trí của các chữ trong từ đó</param>
    public delegate void CorrectWord(string word, List<int> squareIndex);
    public static event CorrectWord OnCorrectWord;

    public static void CorrectWordMetod(string word, List<int> squareIndex)
    {
        if(OnCorrectWord != null)
        {
            OnCorrectWord(word, squareIndex);
        }
    }

    /// <summary>
    /// Định nghĩa logic tiếp tục màn chơi
    /// </summary>
    public delegate void BoardCompleted();
    public static event BoardCompleted OnBoardCompleted;

    public static void BoardCompletedMethod()
    {
        if (OnBoardCompleted != null)
        {
            OnBoardCompleted();
        }
    }

    /// <summary>
    /// Định nghĩa logic mở khoa cấp độ tiếp theo
    /// </summary>
    public delegate void UnlockNextCategory();
    public static event UnlockNextCategory OnUnlockNextCategory;

    public static void UnlockNextCategoryMethod()
    {
        if (OnUnlockNextCategory != null)
        {
            OnUnlockNextCategory();
        }
    }

    /// <summary>
    /// Định nghĩa logic load next level
    /// </summary>
    public delegate void LoadNextLevel();
    public static event LoadNextLevel OnLoadNextLevel;

    public static void LoadNextLevelMethod()
    {
        if (OnLoadNextLevel != null)
        {
            OnLoadNextLevel();
        }
    }
}
