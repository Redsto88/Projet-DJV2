using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnigme : AProjectile
{


    private Rigidbody rb;
    public Transform sphere;

    public float speed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * speed);
        //sphere roll
        sphere.Rotate(Vector3.right * Time.deltaTime * 20*speed);
        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
