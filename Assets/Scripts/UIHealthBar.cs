using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{

    [SerializeField] private Image _health;

    private float _maxHealth;

    private void Start()
    {
        _maxHealth = PlayerManager.Instance.GetHealthMax();
    }

    public void SetHealth(float health)
    {
        _health.rectTransform.anchorMax = new Vector2(health / _maxHealth, 1);
    }
}
