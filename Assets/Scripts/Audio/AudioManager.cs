using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musics, sfx;
    public AudioSource musicSource, sfxSource;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic("Theme")
    }
    
    public void PlayMusic(string name, bool loop = false)
    {
        Sound s = Array.Find(musics, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
            musicSource.loop = loop;

            if (loop) return;
            switch (name)
            {
                case "Boss_Intro":
                    StartCoroutine(NextMusic(s.clip, "Boss_Corps",true,0.85f));
                    break;
                default:
                    StartCoroutine(NextMusic(s.clip, "Theme"));
                    break;
            }
        }
    }

    IEnumerator NextMusic(AudioClip clip, string name, bool loop = false, float control = 0)
    {
        yield return new WaitForSeconds(clip.length-control);
        print(clip.length);
        PlayMusic(name,loop);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfx, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX not found");
        }

        else
        {
           sfxSource.PlayOneShot(s.clip);
        }

    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
