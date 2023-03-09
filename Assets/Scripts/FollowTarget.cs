using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public bool aimPoint;
    public Vector3 targetPoint;
    private Vector3 _currentVelocity;
    
    protected void Update()
    {
        if (aimPoint && targetPoint != null)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPoint,
                ref _currentVelocity,
                0.2f);
                return;
        }
        if(target != null){
            transform.position = Vector3.SmoothDamp(
                transform.position,
                target.position,
                ref _currentVelocity,
                0.2f);
        }
        else{
            target = PlayerManager.Instance?.transform;
        }
    }
}
