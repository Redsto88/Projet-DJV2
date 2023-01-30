using UnityEngine;

public interface IDamageable
{ 
    void ApplyDamaged(float damage);

    float GetHealth();
    
    float GetHealthMax();
}

