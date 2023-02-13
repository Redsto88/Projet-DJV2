using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ACollectable
{
    protected override void OnCollect()
    {
        PlayerManager.Instance.keyCount++;
        StartCoroutine(OnCollectCoroutine());
    }
}

