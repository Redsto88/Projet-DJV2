using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartAnim : MonoBehaviour
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.Play("Base Layer.Anim",0,Random.Range(0.0f,1.0f));
    }
}
