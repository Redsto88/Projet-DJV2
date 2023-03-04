using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleManager1 : MonoBehaviour
{

    public SphereDetector[] detectors;

    public BasicEnemyBehaviour enemy;

    // Update is called once per frame
    void Update()
    {
        bool isFinished = true;
        foreach(var detector in detectors)
        {
            if (!detector.isActivated)
            {
                isFinished = false;
                break;
            }
        }
        if(Input.GetKey(KeyCode.Space)){
            isFinished = true;
        }
        if (isFinished)
        {
            foreach (var detector in detectors)
            {
                detector.material.color = Color.green * 5f;
                detector.light.color = Color.green;
            }

            enemy.ApplyDamaged(1000);
        }
    }
}
