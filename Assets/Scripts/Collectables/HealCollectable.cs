using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollectable : ACollectable
{

    [SerializeField] private float healAmount = 15f;
    [SerializeField] private GameObject healExt;
    [SerializeField] private GameObject healInt;

    private void Update()
    {
        healExt.transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        healInt.transform.Rotate(Vector3.up, -10 * Time.deltaTime);
    }

    protected override void OnCollect()
    {
        print("heal");
        PlayerManager.Instance.ApplyDamage(-healAmount);
        AudioManager.Instance.PlaySFX("Heal");
        StartCoroutine(OnCollectCoroutine());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        if (other.TryGetComponent(out PlayerManager pC))
        {
            if (pC.IsFullHealth())
            {
                print("player is full life");
                return;
            }
            OnCollect();
        }
    }


}
