using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DistanceEnemyBehaviour : BasicEnemyBehaviour
{

    [SerializeField] private GameObject _bulletPrefab;

    public float _cooldown = 2f;
    public float _cooldownTimer = 0f;
    
    


    // Update is called once per frame
    void Update()
    {
        if (!portalFlag && !_target.IsUnityNull())
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                navMeshAgent.destination = _target.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
                //rotate to target
                Vector3 targetDir = _target.position - transform.position;
                float step = 5 * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDir);
                
            }
            if(_cooldownTimer >= _cooldown)
            {
                Shoot();
                _cooldownTimer = 0f;
            }
            else _cooldownTimer += Time.deltaTime;
        }

    }

    private void Shoot()
    {
        if (portalFlag)
        {
            return;
        }
        
        GameObject bullet = Instantiate(_bulletPrefab, transform.position + transform.forward*1.5f, transform.rotation);
        bullet.transform.position = new Vector3(bullet.transform.position.x, 1f, bullet.transform.position.z);
        bullet.GetComponent<Bullet>().target = _target;
        bullet.gameObject.SetActive(true);
    }
    
}
