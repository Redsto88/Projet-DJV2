using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    private Transform _target;

    public bool portalFlag;
    
   // public float speed;
   // public float minDist;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _target = PlayerController.Instance.transform;
        portalFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
/*
        var dir = (PlayerController.Instance.transform.position - transform.position);
        if (dir.magnitude > minDist)
        {
            dir = dir.normalized;
            transform.position = transform.position + dir * (speed * Time.deltaTime);
        }
        var rot = transform.rotation;
        transform.LookAt(PlayerController.Instance.transform);
        transform.rotation = Quaternion.RotateTowards(rot, transform.rotation, 180);*/

        print(portalFlag);
        if (!portalFlag)
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                navMeshAgent.destination = _target.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
            }
        }
    }
}
