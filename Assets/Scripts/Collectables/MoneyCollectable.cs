using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectable : ACollectable
{
    [SerializeField] private float moneyAmount = 15f;


    protected override void OnCollect()
    {
        PlayerManager.Instance.AddMoney(moneyAmount);
        StartCoroutine(OnCollectCoroutine());
    }
}
