using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private SpawnPortal _spawnPortal;
    
    // Start is called before the first frame update
    void Start()
    {
        _spawnPortal = GetComponent<SpawnPortal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Portal"))
        {
            _spawnPortal.CreatePortal();
        }
        else if(Input.GetButtonDown("DeletePortals"))
        {
            _spawnPortal.DeletePortals();
        }
    }
}
