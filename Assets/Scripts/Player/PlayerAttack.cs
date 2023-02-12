using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private SpawnPortal spawnPortal;
    private Animator _animator;
    
    [SerializeField] private Weapon actualWeapon;
    private Collider _weaponCollider;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _weaponCollider = actualWeapon.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Portal"))
        {
            spawnPortal.CreatePortal();
        }
        else if(Input.GetButtonDown("DeletePortals"))
        {
            spawnPortal.DeletePortals();
        }
        else if (Input.GetButtonDown("Weapon"))
        {
            _animator.CrossFade("Attaque_01", 0.1f);
        }
    }
    
    public void OpenWeaponCollisions()
    {
        print("open collider");
        _weaponCollider.enabled = true;

        if (!actualWeapon.trail.IsUnityNull())
        {
            actualWeapon.trail.Play();
        }

    }
    
    public void CloseWeaponCollisions()
    {
        print("close collider");
        _weaponCollider.enabled = false;
        
        if(!actualWeapon.trail.IsUnityNull())
        {
            actualWeapon.trail.Stop();
        }
        
    }
}
