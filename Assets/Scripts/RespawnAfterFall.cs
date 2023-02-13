using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFall : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    private void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        if (other.TryGetComponent(out IDamageable damageable))
        {
            print("damageable");
            if (other == PlayerController.Instance.characterController)
            {
                print("damage + tp");
                PlayerController.Instance.characterController.enabled = false;
                damageable.ApplyDamaged(10);
                PlayerController.Instance.transform.position = respawnTransform.position;
                PlayerController.Instance.transform.rotation = respawnTransform.rotation;
                PlayerController.Instance.characterController.enabled = true;
            }
        }
    }
}
