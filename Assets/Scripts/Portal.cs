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
        StartCoroutine(OrientationCoroutine());
    }

    IEnumerator OrientationCoroutine()
    {
        while (Input.GetButton("Portal"))
        {
            transform.LookAt(aim.transform.position);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("portal trigger");
        if (!linkPortal.IsUnityNull())
        {
           StartCoroutine(Teleport(other));
        }
    }

    private IEnumerator Teleport(Collider collider)
    {
        print("Teleport");
        linkPortal.GetComponent<BoxCollider>().enabled = false;

        if (collider == PlayerController.Instance.characterController)
        {
            print("collider = player");
            collider.enabled = false;
            collider.gameObject.transform.position = linkPortal.transform.position;
            PlayerController.Instance.playerPivot.transform.rotation = linkPortal.transform.rotation;
            yield return new WaitForEndOfFrame();
            collider.enabled = true;
        }
        else
        {
            print("collider = other");
            collider.gameObject.transform.position = linkPortal.transform.position;
            collider.gameObject.transform.rotation = linkPortal.transform.rotation;
            yield return new WaitForEndOfFrame();
        }
        

        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
    }

    
}
