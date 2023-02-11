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
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void OnTriggerEnter(Collider col)
    {
        print(col);
        if (col.TryGetComponent(out IDamageable damageable))
        {
            damageable.ApplyDamaged(damage);
        }
        
        
    }
}
