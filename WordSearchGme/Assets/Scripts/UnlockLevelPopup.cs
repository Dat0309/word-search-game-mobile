using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevelPopup : MonoBehaviour
{
    /// <summary>
    /// Hàm xuất thông báo lên level trong game
    /// </summary>
    /// 
    [System.Serializable]
    public struct CategoryName
    {
        public string name;
        public Sprite sprite;
    };

    public GameData currentGameData;
    public List<CategoryName> categoryNames;
    public GameObject winPopup;
    public Image categoryNameImage;

    // Start is called before the first frame update
    void Start()
    {
        winPopup.SetActive(false);

        GameEvents.OnUnlockNextCategory += OnUnlockNextCategory;
    }

    private void OnDisable()
    {
        GameEvents.OnUnlockNextCategory -= OnUnlockNextCategory;
    }

    private void OnUnlockNextCategory()
    {
        bool captureNext = false;
        foreach (var writing in categoryNames)
        {
            if (captureNext)
            {
                categoryNameImage.sprite = writing.sprite;
                captureNext = false;
                break;
            }
            if(writing.name == currentGameData.selectedCategoryName)
            {
                captureNext = true;
            }

            winPopup.SetActive(true);
        }
    }
}

