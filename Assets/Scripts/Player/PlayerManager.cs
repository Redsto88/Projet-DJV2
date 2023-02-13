using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    public static PlayerManager Instance;
    [SerializeField] private float healthMax;
    private float _health;

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

    // Start is called before the first frame update
    void Start()
    {
        _health = healthMax;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // IDamageable
    public void ApplyDamaged(float damage)
    {
        _health -= damage;
        if(_health > healthMax)
            _health = healthMax;
        healthBar.SetHealth(_health);
        if (_health <= 0)
        {
            Destroy(gameObject);
            //TODO fin de partie : défaite
        }
    }

    public bool IsFullHealth()
    {
        return _health == healthMax;
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetHealthMax()
    {
        return healthMax;
    }
}
