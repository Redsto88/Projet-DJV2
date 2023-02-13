using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IDamageable
{
    public static PlayerManager Instance;

    [SerializeField] private UIHealthBar healthBar;

    public int keyCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public override void ApplyDamaged(float damage)
    {
        base.ApplyDamaged(damage);
        healthBar.SetHealth(_health);
        //TODO fin de partie : défaite
    }
}
