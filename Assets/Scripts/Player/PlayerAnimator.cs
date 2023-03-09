using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Centraliser les inputs dans un script à part pour faire plus propre avec le PlayerController
        Vector3 direction = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        float speedParam = CinematicManager.cinematicPause ? 0 : Mathf.Clamp01(direction.magnitude);
        _animator.SetFloat(Speed, speedParam);
    }
}
