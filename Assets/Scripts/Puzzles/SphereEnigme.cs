using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SphereEnigme : MonoBehaviour
{


    private Rigidbody _rb;
    public Transform sphere;

    public float speed = 5f;

    public float time = 5f;
    public SphereDetector detector;

    private AudioSource _audioSource;
    private SphereCollider _sphereCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyCoroutine());
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
        _sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //sphere roll
        sphere.Rotate(Vector3.right * (Time.deltaTime * 20 * _rb.velocity.magnitude));
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, _sphereCollider.radius + 0.1f) &&
            _rb.velocity.magnitude > 0.001f)
        {
            _audioSource.volume = 1;
        }
        else
        {
            _audioSource.volume = 0;
        }
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (!detector.IsUnityNull())
        {
            print(detector.light.color);
            if (detector.light.color != Color.green)
            {
                detector.material.color = detector.color;
                detector.light.color = detector.lightColor;
                detector.isActivated = false;
            }
        }
    }
}
