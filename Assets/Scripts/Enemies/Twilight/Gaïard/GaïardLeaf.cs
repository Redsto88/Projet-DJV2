using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public void SetLeaf(float time, Vector3 initVel, bool aimed, Transform target, float angle, float _damage, Animator animator, string stateName, string audio)
    {
        damage = _damage;
        StartCoroutine(stall(time,initVel,aimed,target,angle,animator,stateName, audio));
    }

    IEnumerator stall(float time, Vector3 initVel, bool aimed, Transform target, float angle, Animator animator, string stateName, string audio)
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
        AudioManager.Instance.PlaySFX(audio);
        animator.CrossFade(stateName, 0.1f);
        go = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!go) return;
        if (other.gameObject.TryGetComponent<PlayerManager>(out var id))
        {
            id.ApplyDamage(damage);
            AudioManager.Instance.PlaySFX("LeafDestroy");
            Destroy(gameObject);
        }
        else if (other.gameObject.TryGetComponent<Portal>(out var portal))
        {
            //nothing
        }
        else {
            AudioManager.Instance.PlaySFX("LeafDestroy");
            Destroy(gameObject);
        }
    }
}
