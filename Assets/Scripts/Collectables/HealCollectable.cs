using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCollectable : ACollectable
{

    [SerializeField] private float healAmount = 15f;

    protected override void OnCollect()
    {
        PlayerManager.Instance.ApplyDamaged(-healAmount);
        StartCoroutine(OnCollectCoroutine());
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerManager pC))
        {
            if (pC.IsFullHealth())
            {
                return;
            }
            OnCollect();
        }
    }


}
