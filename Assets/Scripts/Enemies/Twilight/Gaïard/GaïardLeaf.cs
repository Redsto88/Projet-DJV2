using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaïardLeaf : AProjectile
{
    public bool go = false;
    public float shotSpeed;
    public float damage;
    private Vector3 speed;

    // Update is called once per frame
    void Update()
    {
        if (!go) 
        {
            transform.localPosition += speed * Time.deltaTime;
        }
        else 
        {
            transform.position += shotSpeed * Time.deltaTime * transform.forward;
        }
    }

    public void SetLeaf(float time, Vector3 initVel, bool aimed, Transform target, float angle)
    {
        StartCoroutine(stall(time,initVel,aimed,target,angle));
    }

    IEnumerator stall(float time, Vector3 initVel, bool aimed, Transform target, float angle)
    {
        speed = initVel;
        var timeEllapsed = 0f;
        while (timeEllapsed < time)
        {
            speed += Time.deltaTime * new Vector3(Random.Range(-1f,1f),Random.Range(-2f,0.5f),Random.Range(-0.5f,0.5f));
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        transform.LookAt(target.position + Vector3.up);
        if (!aimed) transform.Rotate(angle * Vector3.up);
        transform.SetParent(null);
        go = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!go) return;
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.gameObject.GetComponent<IDamageable>().ApplyDamaged(damage);
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
