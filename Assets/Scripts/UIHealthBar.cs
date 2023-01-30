using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{

    [SerializeField] private Image _health;
    
    public float _max_health = 100;


    public void SetHealth(float health)
    {
        _health.rectTransform.anchorMax = new Vector2(health / _max_health, 1);
    }
}
