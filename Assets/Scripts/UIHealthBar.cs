using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{

    private Slider _health;
    [SerializeField] private ADamageable parent;

    private float _maxHealth;

    private void Start()
    {
        _health = GetComponent<Slider>();
        _maxHealth = parent.GetHealthMax();
    }

    public void SetHealth(float health)
    {
        _health.value = health / _maxHealth;
    }
}
