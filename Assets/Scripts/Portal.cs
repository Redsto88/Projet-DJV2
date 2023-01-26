using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkPortal;
    [SerializeField] private GameObject aim;

    private void OnEnable()
    {
        print("portal enable");
        StartCoroutine(OrientationCoroutine());
    }

    IEnumerator OrientationCoroutine()
    {
        print("coroutine orientation");
        while (Input.GetButton("Portal"))
        {
            print("look at");
            transform.LookAt(aim.transform.position);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!linkPortal.IsUnityNull())
        {
           StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider collider)
    {
        linkPortal.GetComponent<BoxCollider>().enabled = false;
       
       if (collider == PlayerController.Instance.characterController)
        {
            PlayerController.Instance.playerPivot.transform.rotation = linkPortal.transform.rotation;
        }
        
        PlayerController.Instance.characterController.enabled = false;
        collider.gameObject.transform.position = linkPortal.transform.position;
        
        if (collider == PlayerController.Instance.characterController)
        {
            PlayerController.Instance.playerPivot.transform.rotation = linkPortal.transform.rotation;
        }
        else
        {
            collider.gameObject.transform.rotation = linkPortal.transform.rotation;
        }
        
        yield return new WaitForEndOfFrame();
        PlayerController.Instance.characterController.enabled = true;

        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
    }

    
}
