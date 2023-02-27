using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IDamageable
{
    public static PlayerManager Instance;

    [SerializeField] private UIHealthBar healthBar;

    [SerializeField] private float money = 0f;

    public int keyCount = 0;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public override void ApplyDamaged(float damage)
    {
        _health -= damage;
        healthBar.SetHealth(_health);
        if(_health <= 0)
        {
            GameManager.Instance.onPlayerDeath();
            Destroy(gameObject);
        }

        
    }


    public void AddMoney(float money)
    {
        this.money += money;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
