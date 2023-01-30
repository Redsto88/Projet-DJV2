using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
    [SerializeField] private float healthMax;
    private float _health;
    
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

        if (_health <= 0)
        {
            Destroy(gameObject);
            //TODO fin de partie : défaite
        }
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
