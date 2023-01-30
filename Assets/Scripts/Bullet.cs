using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Transform target;
    
    public Vector3 destination;

    void Start()
    {
        if (target != null)
        {
            destination = target.position + Vector3.up;
            transform.LookAt(destination);
        }
        Destroy(gameObject, 15f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 5f;   
    }

    void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name);
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.gameObject.GetComponent<IDamageable>().ApplyDamaged(10f);
            Destroy(gameObject);
        }
        else if (other.gameObject.TryGetComponent<Portal>(out var portal))
        {
            //nothing
        }
        else {
            Destroy(gameObject);
        }
    }
}
