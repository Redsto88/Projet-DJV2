using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Stats")] 
    public float damage;

    [Header("Particules")]
    public ParticleSystem trail;
    
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        if (!trail.IsUnityNull())
        {
            trail.Pause();
        } 
    }
    
    public void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out ADamageable damageable))
        {
            damageable.ApplyDamaged(damage);
        }
        
        
    }


}
