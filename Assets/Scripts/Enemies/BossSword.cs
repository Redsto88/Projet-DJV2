using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSword : MonoBehaviour
{
    [SerializeField] private Weapon actualWeapon;
    private Collider _weaponCollider;
    
    [SerializeField] private Transform impactTransform;
    [SerializeField] private GameObject swordBouleAttaquePS;

    private void Start()
    {
        _weaponCollider = actualWeapon.GetComponent<Collider>();
    }

    public void OpenWeaponCollisions()
    {
        _weaponCollider.enabled = true;
        actualWeapon.trail.Play();
    }
    
    public void CloseWeaponCollisions()
    {
        _weaponCollider.enabled = false;
        actualWeapon.trail.Stop();
    }

    public void PlayPS()
    {
        var ps = Instantiate(swordBouleAttaquePS, impactTransform.position, Quaternion.identity);
    }
}
