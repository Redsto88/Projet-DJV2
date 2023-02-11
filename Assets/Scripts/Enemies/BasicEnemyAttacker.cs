using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAttacker : MonoBehaviour
{
    [SerializeField] private Collider weaponCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenWeaponCollisions()
    {
        weaponCollider.enabled = true;
    }
    
    public void CloseWeaponCollisions()
    {
        weaponCollider.enabled = false;
    }
}
