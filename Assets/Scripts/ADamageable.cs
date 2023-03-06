using UnityEngine;

public abstract class ADamageable : MonoBehaviour
{ 
    [Header("Stats")]
    [SerializeField] protected float _health;
    [SerializeField] protected float healthMax; 

    void Start()
    {
        _health = healthMax;
    }

    public virtual void ApplyDamage(float damage)
    {
        _health -= damage;
        if(_health > healthMax)
            _health = healthMax;
        if (_health <= 0)
        {
            //Death();
            Destroy(gameObject);
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
    
    public float GetHealth()
    {
        return _health;
    }
    
    public float GetHealthMax()
    {
        return healthMax;
    }

    public bool IsFullHealth()
    {
        return _health == healthMax;
    }
}

