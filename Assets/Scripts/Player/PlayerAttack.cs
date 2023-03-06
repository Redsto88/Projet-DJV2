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
    public bool canAttack;
    public bool _canCombo = false;
    public bool _canPreCombo = false;
    public bool _hasAttackBeforeCombo = false;
    public string _lastAttack = "";
    

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _weaponCollider = actualWeapon.GetComponent<Collider>();
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canAttack) return;
        if (Input.GetButtonDown("Portal"))
        {
            spawnPortal.CreatePortal();
        }
        else if (Input.GetButtonDown("DeletePortals"))
        {
            spawnPortal.DeletePortals();
        }
        else if (Input.GetButtonDown("Weapon"))
        {
            switch (_lastAttack)
            {
                case "Attack_01":
                    if (_canCombo)
                    {
                        _animator.CrossFade("Attaque_02", 0.1f);
                        _lastAttack = "Attack_02";
                    }
                    else if (_canPreCombo)
                    {
                        _hasAttackBeforeCombo = true;
                    }
                    else
                    {
                        return;
                    }

                    break;

                case "Attack_02":
                    if (_canCombo)
                    {
                        _animator.CrossFade("Attaque_03", 0.1f);
                        _lastAttack = "Attack_03";
                    }
                    else if (_canPreCombo)
                    {
                        _hasAttackBeforeCombo = true;
                    }
                    else
                    {
                        return;
                    }

                    break;

                case "Attack_03":
                    return;

                default:
                {

                    _animator.CrossFade("Attaque_01", 0.1f);
                    _lastAttack = "Attack_01";
                    break;
                }
            }
        }
    }
    
    public void OpenWeaponCollisions()
    {
        print("open collider");
        
        if (_lastAttack == "Attack_03")
        {
            actualWeapon.damage *= 1.5f;
        }
        _weaponCollider.enabled = true;

        if (!actualWeapon.trail.IsUnityNull())
        {
            actualWeapon.trail.Play();
        }

    }
    
    public void CloseWeaponCollisions()
    {
        print("close collider");
        
        if (_lastAttack == "Attack_03")
        {
            actualWeapon.damage /= 1.5f;
            _lastAttack = "";
        }
        
        _weaponCollider.enabled = false;
        
        if(!actualWeapon.trail.IsUnityNull())
        {
            actualWeapon.trail.Stop();
        }
        
    }

    public void OpenCombo()
    {
        if (_hasAttackBeforeCombo)
        {
            switch (_lastAttack)
            {
                case "Attack_01" :
                    _hasAttackBeforeCombo = false;
                    _animator.CrossFade("Attaque_02", 0.1f);
                    _lastAttack = "Attack_02";
                    return;
                case "Attack_02" :
                    _hasAttackBeforeCombo = false;
                    _animator.CrossFade("Attaque_03", 0.1f);
                    _lastAttack = "Attack_03";
                    return;
                default:
                    return;
            }
        }
        _canCombo = true;
    }
    
    public void CloseCombo()
    {
        _lastAttack = "";
        _canCombo = false;
    }
    
    public void OpenPreCombo()
    {
        _canPreCombo = true;
    }
    
    public void ClosePreCombo()
    {
        _canPreCombo = false;
    }
    
    public void DisableMove()
    {
        PlayerController.Instance.canMove = false;
    }

    public void EnableMove()
    {
        PlayerController.Instance.canMove = true;
    }
}
