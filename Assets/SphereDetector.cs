using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDetector : MonoBehaviour
{


    public bool isActivated = false;
    private int count = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to green
            GetComponent<Renderer>().material.color = Color.green;
            count++;
            isActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to red
            count--;
            if (count <0) count = 0;
            else if (count == 0)
            {
                GetComponent<Renderer>().material.color = Color.red;
                isActivated = false;
            }
        }
    }
}