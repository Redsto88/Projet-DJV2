using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFall : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    private void OnTriggerEnter(Collider other)
    {
        print("true");
        if (other.TryGetComponent(out IDamageable damageable))
        {
            if (other == PlayerController.Instance.characterController)
            {
                damageable.ApplyDamaged(10);
                PlayerController.Instance.transform.position = respawnTransform.position;
                PlayerController.Instance.transform.rotation = respawnTransform.rotation;
            }
        }
    }
}
