using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private bool loop = false;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(_name, loop); 
        Destroy(gameObject);
    }
}
