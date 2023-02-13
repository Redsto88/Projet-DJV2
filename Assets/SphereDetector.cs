using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to green
            GetComponent<Renderer>().material.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to red
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
}