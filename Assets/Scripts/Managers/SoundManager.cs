using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
                if (_instance == null)
                {
                    Debug.LogError("Error SoundManager");
                }
            }
            return _instance;
        }
    }

    public AudioSource musicSource;
    public AudioSource effectSource;
    public AudioSource monsterSource;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayEffect(AudioClip clip) 
    {
        effectSource.clip = clip;
        effectSource.Play();
    }
    public void PlayMonsterEffect(AudioClip clip)
    {
        monsterSource.clip = clip;
        monsterSource.Play();
    }
}
