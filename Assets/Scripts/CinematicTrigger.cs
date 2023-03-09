using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    [SerializeField] private int cinematicId;
    [SerializeField] private bool launchOnce;
    private bool hasBeenLaunched;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && (!hasBeenLaunched || !launchOnce))
        {
            CinematicManager.Instance.StartCinematic(cinematicId);
            hasBeenLaunched = true;
        }
    }
}
