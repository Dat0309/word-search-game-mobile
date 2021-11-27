using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Lớp quản lý các âm thanh trong trò chơi
    /// </summary>
    /// 

    private bool _muteBackgroundMusic = false;
    private bool _muteSoundFx = false;
    public static SoundManager instance;
    public AudioClip winSound;
    public AudioClip loseSound;

    private AudioSource _audioSource;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    public void ToggleBackgroundMusic()
    {
        _muteBackgroundMusic = !_muteBackgroundMusic;
        if (_muteBackgroundMusic)
        {
            _audioSource.Stop();
        }
        else
        {
            _audioSource.Play();
        }
    }

    public void ToggleSoundFX()
    {
        _muteSoundFx = !_muteSoundFx;
        GameEvents.OnToggleSoundFXMethod();
    }

    public bool isBackgroundMusicMuted()
    {
        return _muteBackgroundMusic;
    }

    public bool IsSoundFXMuted()
    {
        return _muteSoundFx;
    }

    public void SilienceBackgroundMusic(bool silience)
    {
        if(_muteBackgroundMusic == false)
        {
            if (silience)
            {
                _audioSource.volume = 0f;
            }
            else _audioSource.volume = 1f;
        }
    }

    public void PlaySound(AudioClip sound)
    {
        if(_audioSource && sound)
            _audioSource.PlayOneShot(sound);

    }

    /// <summary>
    /// Phát nhạc khi thua
    /// </summary>
    public void PlayWinSound()
    {
        PlaySound(winSound);
    }

    /// <summary>
    /// Phát nhạc khi thắng
    /// </summary>
    public void PlayLoseSound()
    {
        PlaySound(loseSound);
    }
}
