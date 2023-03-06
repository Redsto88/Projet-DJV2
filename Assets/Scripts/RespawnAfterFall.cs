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
        if (other.TryGetComponent(out ADamageable damageable))
        {
            print("damageable");
            if (other == PlayerController.Instance.characterController)
            {
                print("portalFlag true");
                PlayerController.Instance.respawnFlag = true;
                PlayerController.Instance.characterController.enabled = false;
                print("damage");
                damageable.ApplyDamage(10);
                print("position");
                PlayerController.Instance.transform.position = respawnTransform.position;
                print("rotation");
                PlayerController.Instance.playerPivot.transform.rotation = respawnTransform.rotation;
                print("portalFlag false");
                PlayerController.Instance.respawnFlag = false;
                PlayerController.Instance.characterController.enabled = true;
                print("end");
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
