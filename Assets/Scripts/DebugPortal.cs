using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugPortal : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    // Update is called once per frame
    void Update()
    {
        if (agent.enabled)
        {
            transform.position = agent.destination;
        }
    }
}
