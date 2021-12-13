using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Text.RegularExpressions;

[CustomEditor(typeof(BoardData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    // Tạo một hàng dư liệu
    private BoardData GameDataInstance => target as BoardData;

    // List lưu trữ các Serializable
    private ReorderableList _dataList;

    // Tìm ra các property của các biến và gán cho những biến vừa tạo

    private void OnEnable()
    {

    }

    /*Ghi đè phương thức OnInspectorGUI để hiển thị tất cả những biến và nút ở trên, việc này giúp tạo ra 1 setting
    để lưu trữ trực tiếp trên inspector và nếu muốn sửa thì không phải sửa ở code.
    Dùng làm level cho game rất hiệu quả!
    
     */
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameDataInstance.timeInSeconds = EditorGUILayout.FloatField("Max Game Time (in Seconds)", GameDataInstance.timeInSeconds);

        DrawColumsRowsInputFields();
        EditorGUILayout.Space();
        ConvertToUpperButton();

        if (GameDataInstance.Board != null && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
            DrawBoardTable();

        GUILayout.BeginHorizontal();
        ClearBoardButton();
        FillUpWithRandomLetterButton();
        FillKeyUpWithRandomLetterButton();

        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        // Tạo list các từ cần tìm
        InitializeReordableList(ref _dataList, "SearchWords", "Searching Words");
        _dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    // EditorGUILayout: Khởi tạo các trường nhập dữ liệu để thay đổi số hàng, cột
    private void DrawColumsRowsInputFields()
    {
        var columnsTemp = GameDataInstance.Columns;
        var rowsTemp = GameDataInstance.Rows;

        GameDataInstance.Columns = EditorGUILayout.IntField("Columns", GameDataInstance.Columns);
        GameDataInstance.Rows = EditorGUILayout.IntField("Rows", GameDataInstance.Rows);

        if ((GameDataInstance.Columns != columnsTemp || GameDataInstance.Rows != rowsTemp)
            && GameDataInstance.Columns > 0
            && GameDataInstance.Rows > 0)
        {
            GameDataInstance.CreateNewBoard();
        }
    }

    // Tạo bảng dữ liệu dựa trên số hàng (Rows) và số cột (Columns) đã được nhập
    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 50;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var textFieldStyle = new GUIStyle();

        textFieldStyle.normal.background = Texture2D.grayTexture;
        textFieldStyle.normal.textColor = Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        // Vẽ hàng ngang
        EditorGUILayout.BeginHorizontal(tableStyle);
        for (var x = 0; x < GameDataInstance.Columns; x++)
        {
            // Vẽ hàng dọc
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for (var y = 0; y < GameDataInstance.Rows; y++)
            {
                if (x >= 0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y], textFieldStyle);
                    if (GameDataInstance.Board[x].Row[y].Length > 1)
                    {
                        character = GameDataInstance.Board[x].Row[y].Substring(0, 1);
                    }
                    GameDataInstance.Board[x].Row[y] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void InitializeReordableList(ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);
        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Word"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 230, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("WordValue"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 460, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Image"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 920, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Voice"), GUIContent.none);

        };
    }

    // Chuyển tất cả các chữ cái thành upper case
    private void ConvertToUpperButton()
    {
        if (GUILayout.Button("To Upper"))
        {
            for (var i = 0; i < GameDataInstance.Columns; i++)
            {
                for (var j = 0; j < GameDataInstance.Rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-z]").Count;

                    if (errorCounter > 0)
                    {
                        GameDataInstance.Board[i].Row[j] = GameDataInstance.Board[i].Row[j].ToUpper();
                    }
                }
            }

            foreach (var searchWord in GameDataInstance.SearchWords)
            {
                var errorCounter = Regex.Matches(searchWord.Word, @"[a-z]").Count;

                if (errorCounter > 0)
                {
                    searchWord.Word = searchWord.Word.ToUpper();
                }
            }
        }
    }

    // Xoá bảng
    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            for (int i = 0; i < GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].Row[j] = "";
                }
            }
        }
    }

    // Thêm random các chữ cái vào bảng vào bảng
    private void FillUpWithRandomLetterButton()
    {
        if (GUILayout.Button("Fill Up With Random"))
        {
            for (int i = 0; i < GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    int errorCounter = Regex.Matches(GameDataInstance.Board[i].Row[j], @"[a-zA-Z]").Count;
                    string letters = "QWERTYUIOPLKJHGFDSAZXCVBNM";
                    int index = UnityEngine.Random.Range(0, letters.Length);

                    if (errorCounter == 0)
                    {
                        GameDataInstance.Board[i].Row[j] = letters[index].ToString();
                    }
                }
            }
        }
    }

    // Thêm random các từ khoá vào bảng vào bảng
    private void FillKeyUpWithRandomLetterButton()
    {
        char[,] newGrid = new char[GameDataInstance.Rows, GameDataInstance.Columns];

        if (GUILayout.Button("Fill Key Up With Random"))
        {
            foreach (var word in GameDataInstance.SearchWords)
            {
                //for (int i = 0; i < word.Word.Length; i++)
                //{
                bool canPlace = true;
                while (canPlace)
                {
                    // random cords x and y in the board
                    int placeCordX = Random.Range(0, GameDataInstance.Rows);
                    int placeCordY = Random.Range(0, GameDataInstance.Columns);

                    void PlaceHorizontal()
                    {
                        // check if can place the word
                        if (word.Word.Length + placeCordX < GameDataInstance.Board.Length)
                        {
                            if (IsValidForHorz(true, word.Word, placeCordX, placeCordY, newGrid))
                            {

                                PlaceWord(true, word.Word, placeCordX, placeCordY, 1, 0, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                    void PlaceVertical()
                    {
                        if (word.Word.Length + placeCordY < GameDataInstance.Board.Length)
                        {
                            if (IsValidForVert(true, word.Word, placeCordX, placeCordY, newGrid))
                            {
                                PlaceWord(true, word.Word, placeCordX, placeCordY, 0, 1, newGrid);
                                canPlace = false;
                            }

                        }
                    }
                    void PositiveDiagonals()
                    {
                        if (word.Word.Length + placeCordX < GameDataInstance.Rows
                            && word.Word.Length + placeCordY < GameDataInstance.Columns)
                        {
                            if (isValidForDiagonalPos(true, word.Word, placeCordX, placeCordY, newGrid))
                            {
                                PlaceWord(true, word.Word, placeCordX, placeCordY, 1, 1, newGrid);
                                canPlace = false;
                            }
                        }
                    }
                    void NegativeDiagonals()
                    {
                        if (word.Word.Length + placeCordX < GameDataInstance.Rows &&
                            word.Word.Length + placeCordY < GameDataInstance.Columns &&
                            placeCordX > word.Word.Length && placeCordY > word.Word.Length)
                        {
                            if (IsValidForDiagonalNeg(true, word.Word, placeCordX, placeCordY, newGrid))
                            {
                                PlaceWord(true, word.Word, placeCordX, placeCordY, 1, -1, newGrid);
                                canPlace = false;
                            }
                        }
                    }

                    // 0 - horizontal
                    // 1 - vertical
                    // 2 - pos diagonal
                    // 3 - neg diagonal
                    int howToPlace = Random.Range(0, 4);

                    if (howToPlace == 0)
                        PlaceHorizontal();
                    if (howToPlace == 1)
                        PlaceVertical();
                    if (howToPlace == 2)
                        PositiveDiagonals();
                    if (howToPlace == 3)
                        NegativeDiagonals();
                    //}
                }
            }

            for (int i = 0; i < GameDataInstance.Columns; i++)
            {
                for (int j = 0; j < GameDataInstance.Rows; j++)
                {
                    GameDataInstance.Board[i].Row[j] = newGrid[i, j].ToString();
                }
            }
        }
    }

    private void PlaceWord(bool placeForwards, string word, int startX, int StartY, int xSlope, int ySlope, char[,] newGrid)
    {
        if (placeForwards)
            for (int i = 0; i < word.Length; i++)
                newGrid[startX + i * xSlope, StartY + i * ySlope] = word[i];
        else
            for (int i = 0; i < word.Length; i++)
                newGrid[startX + i * xSlope, StartY + i * ySlope] = word[word.Length - 1 - i];
    }

    #region checks if the word can be placed?
    /// <summary>
    /// Kiểm tra hàng ngang
    /// </summary>
    /// <param name="checkForward"></param>
    /// <param name="word"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private bool IsValidForHorz(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Kiểm tra hàng dọc
    /// </summary>
    /// <param name="checkForward"></param>
    /// <param name="word"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private bool IsValidForVert(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX].Row[startY + i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX, startY + i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX].Row[startY + i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX, startY + i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Kiểm tra hàng chéo phải
    /// </summary>
    /// <param name="checkForward"></param>
    /// <param name="word"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private bool isValidForDiagonalPos(bool checkForward, string word, int startX, int startY, char[,] grid)
    {

        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY + i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY + i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY + i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY + i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Kiểm tra hàng chèo trái
    /// </summary>
    /// <param name="checkForward"></param>
    /// <param name="word"></param>
    /// <param name="startX"></param>
    /// <param name="startY"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    private bool IsValidForDiagonalNeg(bool checkForward, string word, int startX, int startY, char[,] grid)
    {
        if (checkForward)
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY - i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY - i] != (int)(word[i]))
                        return false;
            }
        }
        else
        {
            for (int i = 0; i < word.Length; i++)
            {
                int errorCounter = Regex.Matches(GameDataInstance.Board[startX + i].Row[startY - i], @"[a-zA-Z]").Count;
                if (errorCounter != 0)
                    if ((int)grid[startX + i, startY - i] != (int)(word[word.Length - 1 - i]))
                        return false;
            }
        }
        return true;
    }
    #endregion
}
