using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordChecker : MonoBehaviour
{
    /// <summary>
    /// Định nghĩa dữ liệu hiện tại của màn chơi.
    /// </summary>
    public GameData currentGameData;
    public GameLevelData levelData;
    private string _word;

    private int _assignedPoints = 0;
    private int _completeWords = 0;
    /// <summary>
    /// Để tượng tác với các vật thể bằng chuột(chạm) 
    /// cần sử dụng Raycast để phát hiện chúng
    /// </summary>
    private Ray _rayUp, _rayDown;
    private Ray _rayLeft, _rayRight;
    private Ray _rayDiagonalLeftUp, _rayDiagonalLeftDown;
    private Ray _rayDiagonalRightUp, _rayDiagonalRightDown;
    private Ray _currentRay = new Ray();
    private Vector3 _rayStartPosition;
    private List<int> _correctSquareList = new List<int>();

    private void OnEnable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckSquare += SquareSelected;
        GameEvents.OnClearSelection += ClearSelection;
        GameEvents.OnLoadNextLevel += LoadNextGameLevel;
    }

    private void Start()
    {
        _assignedPoints = 0;
        _completeWords = 0;
    }

    private void Update()
    {
        if (_assignedPoints > 0 && Application.isEditor)
        {
            Debug.DrawRay(_rayUp.origin, _rayUp.direction * 4);
            Debug.DrawRay(_rayDown.origin, _rayDown.direction * 4);
            Debug.DrawRay(_rayLeft.origin, _rayLeft.direction * 4);
            Debug.DrawRay(_rayRight.origin, _rayRight.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftUp.origin, _rayDiagonalLeftUp.direction * 4);
            Debug.DrawRay(_rayDiagonalLeftDown.origin, _rayDiagonalLeftDown.direction * 4);
            Debug.DrawRay(_rayDiagonalRightUp.origin, _rayDiagonalRightUp.direction * 4);
            Debug.DrawRay(_rayDiagonalRightDown.origin, _rayDiagonalRightDown.direction * 4);

        }
        
    }

    /// <summary>
    /// Hàm định nghĩa các hướng được chọn của các từ
    /// </summary>
    /// <param name="letter"></param>
    /// <param name="squarePosition"></param>
    /// <param name="squareIndex"></param>
    private void SquareSelected(string letter, Vector3 squarePosition, int squareIndex)
    {
        if (_assignedPoints == 0)
        {
            _rayStartPosition = squarePosition;
            _correctSquareList.Add(squareIndex);
            _word += letter;
            _rayUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, 1));
            _rayDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(0f, -1));
            _rayLeft = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 0f));
            _rayRight = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 0f));
            _rayDiagonalLeftUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, 1));
            _rayDiagonalLeftDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(-1, -1));
            _rayDiagonalRightUp = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, 1));
            _rayDiagonalRightDown = new Ray(new Vector2(squarePosition.x, squarePosition.y), new Vector2(1, -1));
        }
        else if (_assignedPoints == 1)
        {
            _correctSquareList.Add(squareIndex);
            _currentRay = SelectRay(_rayStartPosition, squarePosition);
            GameEvents.SelectSquareMethod(squarePosition);
            _word += letter;
            CheckWord();
        }
        else
        {
            if (IsPointOnTheRay(_currentRay, squarePosition))
            {
                _correctSquareList.Add(squareIndex);
                GameEvents.SelectSquareMethod(squarePosition);
                _word += letter;
                CheckWord();
            }
        }
        _assignedPoints++;
        
    }

    /// <summary>
    /// Hàm kiểm tra từ đã chọn có đúng không
    /// </summary>
    private void CheckWord()
    {
        foreach (var searchingWord in currentGameData.selectedBoardData.SearchWords)
        {
            if (_word == searchingWord.Word)
            {
                GameEvents.CorrectWordMetod(_word, _correctSquareList);
                _completeWords++;
                _word = string.Empty;
                _correctSquareList.Clear();
                CheckBoardCompleted();
                return;
            }
        }
    }

    private bool IsPointOnTheRay(Ray currentRay, Vector3 point)
    {
        var hits = Physics.RaycastAll(currentRay, 100.0f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.position == point)
                return true;
        }
        return false;
    }
    private Ray SelectRay(Vector2 firstPosition, Vector2 secondPosition)
    {
        var direction = (secondPosition - firstPosition).normalized;
        float tolerance = 0.01f;
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - 1f) < tolerance)
        {
            return _rayUp;
        }
        if (Math.Abs(direction.x) < tolerance && Math.Abs(direction.y - (-1f)) < tolerance)
        {
            return _rayDown;
        }
        if (Math.Abs(direction.x - (-1f)) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return _rayLeft;
        }
        if (Math.Abs(direction.x - 1f) < tolerance && Math.Abs(direction.y) < tolerance)
        {
            return _rayRight;
        }
        if (direction.x < 0f && direction.y > 0f)
        {
            return _rayDiagonalLeftUp;
        }
        if (direction.x < 0f && direction.y < 0f)
        {
            return _rayDiagonalLeftDown;
        }
        if (direction.x > 0f && direction.y > 0f)
        {
            return _rayDiagonalRightUp;
        }
        if (direction.x > 0f && direction.y < 0f)
        {
            return _rayDiagonalRightDown ;
        }
        return _rayDown;
    }

    private void ClearSelection()
    {
        _assignedPoints = 0;
        _correctSquareList.Clear();
        _word = string.Empty;
    }

    /// <summary>
    /// Hàm chuyển sang level tiếp theo
    /// </summary>
    private void CheckBoardCompleted()
    {
        bool loadNextCat = false;
        if(currentGameData.selectedBoardData.SearchWords.Count == _completeWords)
        {
            //Lưu tiến trình cấp độ chơi lên progresBar
            var categoryName = currentGameData.selectedCategoryName;
            var currentBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(categoryName);
            var nextBoardIndex = -1;
            var currentCategoryIndex = 0;
            bool readNextLevelName = false;

            for(int index = 0; index< levelData.data.Count; index++)
            {
                if (readNextLevelName)
                {
                    nextBoardIndex = DataSaver.ReadCategoryCurrentIndexValues(levelData.data[index].CatName);
                    readNextLevelName = false;
                }

                if(levelData.data[index].CatName == categoryName)
                {
                    readNextLevelName = true;
                    currentCategoryIndex = index;
                }
            }

            var currentLevelSize = levelData.data[currentCategoryIndex].boardData.Count;
            if(currentBoardIndex < currentLevelSize)
            {
                currentBoardIndex += 1;
            }
            DataSaver.SaveCategoryData(categoryName, currentBoardIndex);

            //Unlock Next category
            if(currentBoardIndex >= currentLevelSize)
            {
                currentCategoryIndex++;
                //Nếu không phải là màn chơi cuối
                if(currentCategoryIndex < levelData.data.Count)
                {
                    categoryName = levelData.data[currentCategoryIndex].CatName;
                    currentBoardIndex = 0;
                    loadNextCat = true;

                    if (nextBoardIndex <= 0)
                        DataSaver.SaveCategoryData(categoryName, currentBoardIndex);

                }
                else if(currentCategoryIndex == levelData.data.Count)
                {
                    SceneManager.LoadScene("SelectCategory");
                }
            }
            else
            {
                GameEvents.BoardCompletedMethod();
            }

            if (loadNextCat)
            {
                GameEvents.UnlockNextCategoryMethod();
            }
        }
    }
    private void LoadNextGameLevel()
    {
        SceneManager.LoadScene("GameScene");
    }
}
