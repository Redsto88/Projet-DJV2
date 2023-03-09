using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Gaïard : BasicEnemyBehaviour
{
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private float damage = 10;
    [SerializeField] private int leafNumber = 3;
    [SerializeField] private float idealDistance;
    [SerializeField] private float aimChance;
    [SerializeField] private float changeDirectionFrequency;
    [SerializeField] private float attackFrequency;
    private float rotationDir = 1;
    // Start is called before the first frame update
    protected override void Start()
    {   
        base.Start();
        StartCoroutine(changeDirection());
        StartCoroutine(leafAttack());
    }

    // Update is called once per frame
    void Update()
    {
        if (!portalFlag && !_target.IsUnityNull())
        {
            var dir = rotationDir * transform.right;
            var offset = _target.position - transform.position;
            dir += transform.forward * (offset.magnitude - idealDistance);
            dir = dir.normalized;
            navMeshAgent.destination = transform.position + dir;
        }
        transform.LookAt(_target);
    }

    IEnumerator changeDirection()
    {
        var olddir = rotationDir;
        var newdir = Random.Range(0,2) * 2 - 1;
        var timeEllapsed = 0f;
        while (timeEllapsed < 0.5f)
        {
            rotationDir = Mathf.Lerp(olddir, newdir, timeEllapsed*2f);
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        rotationDir = newdir;
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(changeDirection());
    }

    IEnumerator leafAttack()
    {
        yield return new WaitForSeconds(attackFrequency);
        if (Random.Range(0f,1f) < aimChance)
        {
            for(int i = 0;i<leafNumber;i++)
            {
                yield return new WaitForSeconds(0.1f);
                var l = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
                l.transform.SetParent(transform);
                l.GetComponent<GaïardLeaf>().SetLeaf(3f+0.4f*i, 1.5f * Vector3.up + Random.Range(-0.15f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, true, _target, 0, damage);
            }

        } 
        else 
        {
            for(int i=0;i<leafNumber;i++)
            {
                yield return new WaitForSeconds(0.1f);
                var l = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
                l.transform.SetParent(transform);
                l.GetComponent<GaïardLeaf>().SetLeaf(3f-0.1f*i, 1.5f * Vector3.up + Random.Range(-0.35f+i/leafNumber*0.7f,-0.35f+(i+1)/leafNumber*0.7f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, false, _target, -30+60*i/(leafNumber-1), damage);
            }

        }
        StartCoroutine(leafAttack());
    }
}
