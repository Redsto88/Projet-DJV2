using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehaviour : MonoBehaviour
{
    public float speed;
    public float minDist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dir = (PlayerController.Instance.transform.position - transform.position);
        if (dir.magnitude > minDist)
        {
            dir = dir.normalized;
            transform.position = transform.position + dir * speed * Time.deltaTime;
        }
        var rot = transform.rotation;
        transform.LookAt(PlayerController.Instance.transform);
        transform.rotation = Quaternion.RotateTowards(rot, transform.rotation, 180);
    }
}
