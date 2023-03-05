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

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(DestroyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        //sphere roll
        sphere.Rotate(Vector3.right * (Time.deltaTime * 20 * _rb.velocity.magnitude));
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
            if (detector.material.color == Color.green) return;
            detector.material.color = detector.color;
            detector.light.color = detector.lightColor;
            detector.isActivated = false;
        }
    }
}
