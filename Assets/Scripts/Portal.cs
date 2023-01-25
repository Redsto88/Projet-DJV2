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
           StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider collider)
    {
        print("teleport from " + transform.position + " to " + linkPortal.transform.position);
        
        //linkPortal.GetComponent<BoxCollider>().enabled = false;
        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DisablePortals();
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
        PlayerController.Instance.characterController.enabled = false;
        collider.gameObject.transform.position = linkPortal.transform.position;
        yield return new WaitForEndOfFrame();
        PlayerController.Instance.characterController.enabled = true;

        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
    }
}
