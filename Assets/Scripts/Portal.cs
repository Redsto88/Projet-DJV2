using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkPortal;

    private void OnTriggerEnter(Collider other)
    {
        if (!linkPortal.IsUnityNull())
        {
           Teleport(other);
        }
    }

    private void Teleport(Collider collider)
    {
        print("teleport");
        
        linkPortal.GetComponent<BoxCollider>().enabled = false;

       /* if (collider == PlayerController.Instance.characterController)
        {
            PlayerController.Instance.transform.position = linkPortal.transform.position;
            PlayerController.Instance.playerPivot.transform.rotation = linkPortal.transform.rotation;
        }
        else
        {
            collider.transform.position = linkPortal.transform.position;
            collider.transform.rotation = linkPortal.transform.rotation;
        }*/
        
        collider.transform.position = linkPortal.transform.position;

        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
    }
}
