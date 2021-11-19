using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    /// <summary>
    /// Hàm định nghĩa nút bật, tắt nhạc nền
    /// </summary>
    public enum ButtonType
    {
        BackgroundMusic,
        SoundFx
    };

    public ButtonType type;

    public Sprite onSprite;
    public Sprite offSprite;

    public GameObject button;
    public Vector3 offButtonPosition;
    private Vector3 _onButtonPosition;
    private Image _image;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();

        _image.sprite = onSprite;
        _onButtonPosition = button.GetComponent<RectTransform>().anchoredPosition;
        ToggleButton();
    }

    public void ToggleButton()
    {
        var muted = false;
        if(type == ButtonType.BackgroundMusic)
        {
            muted = SoundManager.instance.isBackgroundMusicMuted();
        }
        else
        {
            muted = SoundManager.instance.IsSoundFXMuted();
        }


        if (muted)
        {
            _image.sprite = offSprite;
            button.GetComponent<RectTransform>().anchoredPosition = offButtonPosition;

        }
        else
        {
            _image.sprite = onSprite;
            button.GetComponent<RectTransform>().anchoredPosition = _onButtonPosition;
        }
    }
}
