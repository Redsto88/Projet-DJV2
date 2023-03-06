using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereDegats : MonoBehaviour
{
    [SerializeField] private float damage = 15f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerManager>(out var playerdamageable))
        {
            playerdamageable.ApplyDamage(damage);
            Destroy(gameObject);
        }
    }
}
