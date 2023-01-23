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
        if (other == PlayerController.Instance.characterController && !linkPortal.IsUnityNull())
        {
           Teleport();
        }
    }

    private void Teleport()
    {
        print("PortalPosition = " + transform.position + "; LinkedPortalPosition = " + linkPortal.transform.position);
        linkPortal.GetComponent<BoxCollider>().enabled = false;
        
        PlayerController.Instance.transform.position = linkPortal.transform.position;
        PlayerController.Instance.playerPivot.transform.rotation = linkPortal.transform.rotation;
        
        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
    }
}
