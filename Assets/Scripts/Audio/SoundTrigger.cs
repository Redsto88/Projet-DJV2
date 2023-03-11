using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private bool loop = false;

    private void Start()
    {
        print("other");
        AudioManager.Instance.PlayMusic(name,loop);
        Destroy(gameObject);
    }
}
