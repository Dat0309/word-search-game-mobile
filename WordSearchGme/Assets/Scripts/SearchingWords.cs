using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingWords : MonoBehaviour
{
    /// <summary>
    /// Lớp định nghĩa logic tìm kiếm và so sánh từ ngữ
    /// </summary>

    public Text displayedText;
    public Image crossLine;

    private string _word;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnEnable()
    {
        GameEvents.OnCorrectWord += CorrectWord;
    }

    private void OnDisable()
    {
        GameEvents.OnCorrectWord -= CorrectWord; 
    }

    public void SetWord(string word)
    {
        _word = word;
        displayedText.text = _word;
    }

    /// <summary>
    /// Đánh dấu từ đã được chọn đúng
    /// </summary>
    /// <param name="word">Từ ngữ</param>
    /// <param name="squareIndex">Các ô chữ chứa những từ</param>
    private void CorrectWord(string word, List<int> squareIndex)
    {
        if(word == _word)
        {
            crossLine.gameObject.SetActive(true);
        }
    }
}
