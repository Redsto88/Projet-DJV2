using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAfterFall : MonoBehaviour
{
    [SerializeField] private Transform respawnTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ADamageable damageable))
        {
            if (other == PlayerController.Instance.characterController)
            {
                PlayerController.Instance.respawnFlag = true;
                PlayerController.Instance.characterController.enabled = false;
                damageable.ApplyDamage(10);
                PlayerController.Instance.transform.position = respawnTransform.position;
                PlayerController.Instance.playerPivot.transform.rotation = respawnTransform.rotation;
                PlayerController.Instance.respawnFlag = false;
                PlayerController.Instance.characterController.enabled = true;
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        
        else if (other.TryGetComponent(out SphereEnigme sphereEnigme))
        {
            Destroy(sphereEnigme.gameObject);
        }

       
        
        
    }
}
