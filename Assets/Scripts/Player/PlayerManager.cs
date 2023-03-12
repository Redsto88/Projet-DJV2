using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ADamageable
{
    public static PlayerManager Instance;

    [SerializeField] private UIHealthBar healthBar;

    [SerializeField] private float money = 0f;

    public int keyCount = 0;
    private Animator _animator;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        _animator = GetComponentInChildren<Animator>();
    }

    public override void ApplyDamage(float damage)
    {
        _health -= damage;
        AudioManager.Instance.PlaySFX("Player_Damage");
        healthBar.SetHealth(_health);
        if(_health <= 0)
        {
            GameManager.Instance.onPlayerDeath();
            Destroy(gameObject);
        }

        if (damage > 0)
        {
            _animator.CrossFade("Damage",0.1f);
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
