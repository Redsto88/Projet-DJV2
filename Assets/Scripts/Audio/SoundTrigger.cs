using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private bool loop = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == PlayerController.Instance.characterController)
        {
            print("iqefgushfboilurgnvpieurngt");
            AudioManager.Instance.PlayMusic(name,loop);
        }
    }
}
