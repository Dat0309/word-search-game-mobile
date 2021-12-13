using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DetailWordPopup : MonoBehaviour
{
    /// <summary>
    /// Lớp hiển thị chi tiết của từ vựng trên mà hình
    /// </summary>
    public GameData currentGameData;
    public GameObject detailWordPopup;
    public Image image;
    public Text text;
    public AudioSource audioSource;
    AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        detailWordPopup.SetActive(false);
    }

    [System.Obsolete]
    private void OnEnable()
    {
        GameEvents.OnDetailWord += ShowGameDetailPopup;
    }

    [System.Obsolete]
    private void OnDisable()
    {
        GameEvents.OnDetailWord -= ShowGameDetailPopup;
    }

    [System.Obsolete]
    private void ShowGameDetailPopup()
    {
        //detailWordPopup.SetActive(true);
        //SoundManager.instance.PlayWinSound();

        foreach (var searchingWord in currentGameData.selectedBoardData.SearchWords)
        {
            if (searchingWord.Found)
            {
                StartCoroutine(DownloadImage(searchingWord.Image));
                StartCoroutine(DownloadMusic(searchingWord.Voice));
                text.text = searchingWord.Word;
                detailWordPopup.SetActive(true);
                //SoundManager.instance.PlayWinSound();
                break;
            }
        }
    }

    [System.Obsolete]
    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
            //gameObject.GetComponent<Image>().sprite = webSprite;
            image.sprite = webSprite;
        }
    }
    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    IEnumerator DownloadMusic(string audioURL)
    {
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioURL, AudioType.MPEG);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            StartCoroutine(DownloadMusic(audioURL));
        }
        else
        {

            audioClip = DownloadHandlerAudioClip.GetContent(request);
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
