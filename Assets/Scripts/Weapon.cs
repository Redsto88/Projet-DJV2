using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")] 
    public float damage;

    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
    }

    public void EnableColisions()
    {
        _collider.enabled = true;
    }
    
    public void DisableColisions()
    {
        _collider.enabled = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamaged(damage);
        }
    }
}
