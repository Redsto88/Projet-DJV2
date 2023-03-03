using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollectable : ACollectable
{

    [SerializeField] private float healAmount = 15f;

    protected override void OnCollect()
    {
        print("heal");
        PlayerManager.Instance.ApplyDamaged(-healAmount);
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
