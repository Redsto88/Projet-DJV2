using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : DistanceEnemyBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PlayerController.Instance.transform);
        if (!portalFlag && !_target.IsUnityNull())
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                navMeshAgent.destination = _target.position;
            }
            
            else if ((transform.position - _target.position).magnitude < navMeshAgent.stoppingDistance - 4)
            {
                
            }
            
            else
            {
                navMeshAgent.destination = transform.position;
                //rotate to target
                /*Vector3 targetDir = _target.position - transform.position;
                float step = 5 * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);*/
                
            }
            if(_cooldownTimer >= _cooldown)
            {
                Shoot();
                _cooldownTimer = 0f;
            }
            else _cooldownTimer += Time.deltaTime;
        }
    }
}
