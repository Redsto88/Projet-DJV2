using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : ADamageable
{
    public static PlayerManager Instance;

    [SerializeField] private UIHealthBar healthBar;

    [SerializeField] private float money = 0f;

    public float maxFocus;
    public float focus;
    public float focusGain;
    public float focusCost;
    public bool isFocused;

    public int keyCount = 0;
    private Animator _animator;

    private void Awake()
    {
        if(Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        focus = maxFocus;
        DontDestroyOnLoad(this.gameObject);
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (isFocused) focus -= Time.unscaledDeltaTime * focusCost;
        else focus = Mathf.Min(maxFocus, focus + Time.unscaledDeltaTime * focusGain);
    }

    public override void ApplyDamage(float damage)
    {
        _health -= damage;
        if (damage > 0)
        {
            AudioManager.Instance.PlaySFX("Player_Damage");
            _animator.CrossFade("Damage",0.1f);
        }
        healthBar.SetHealth(_health);
        
        if (!(_health <= 0)) return;
        
        GameManager.Instance.onPlayerDeath();
        Destroy(gameObject);


    }


    public void AddMoney(float _money)
    {
        this.money += _money;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
