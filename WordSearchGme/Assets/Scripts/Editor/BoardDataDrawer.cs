using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

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
        DrawColumsRowsInputFields();
        EditorGUILayout.Space();

        if (GameDataInstance.Board != null && GameDataInstance.Columns > 0 && GameDataInstance.Rows > 0)
            DrawBoardTable();

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

        if((GameDataInstance.Columns != columnsTemp || GameDataInstance.Rows != rowsTemp)
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
        for(var x = 0; x < GameDataInstance.Columns; x++)
        {
            // Vẽ hàng dọc
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for(var y = 0; y < GameDataInstance.Rows; y++)
            {
                if(x >= 0 && y >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character = (string)EditorGUILayout.TextArea(GameDataInstance.Board[x].Row[y],textFieldStyle);
                    if(GameDataInstance.Board[x].Row[y].Length > 1)
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
}
