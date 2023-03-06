using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderOnBone : ADamageable
{
    [SerializeField] private BasicEnemyBehaviour enemy;

    private void Start()
    {
        _health = Single.MaxValue;
        healthMax = Single.MaxValue;
    }

    public override void ApplyDamage(float damage)
    {
        enemy.ApplyDamage(damage);
    }
}
