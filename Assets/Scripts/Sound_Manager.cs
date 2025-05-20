using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public AudioSource effectSource;
    public AudioSource musicSource;

    public float effectVolume = 1f;
    public float musicVolume = 1f;

    public static Sound_Manager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }
    public void PlayEffect(AudioClip clip)
    {
        
        effectSource.volume = effectVolume;
        effectSource.PlayOneShot(clip);
    }
}
