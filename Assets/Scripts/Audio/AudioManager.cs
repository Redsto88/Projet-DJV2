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
            Debug.Log(name + s.clip.name);
            if(s.clip != musicSource.clip)
            {
                musicSource.clip = s.clip;
                musicSource.Play();
                musicSource.loop = loop;

                if (loop) return;
                switch (name)
                {
                    case "Boss_Intro":
                        StartCoroutine(NextMusic(s.clip, name,"Boss_Corps", true, 0.85f));
                        break;
                    default:
                        StartCoroutine(NextMusic(s.clip, name,"Base", true));
                        break;
                }
            }
        }
    }

    IEnumerator NextMusic(AudioClip clip, string nameCur, string nameNext, bool loop = false, float control = 0)
    {
        yield return new WaitForSeconds(clip.length-control*Time.unscaledDeltaTime);
        Sound s = Array.Find(musics, x => x.name == nameCur);
        if (s.clip == musicSource.clip)
        {
            PlayMusic(nameNext,loop);
        }
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
