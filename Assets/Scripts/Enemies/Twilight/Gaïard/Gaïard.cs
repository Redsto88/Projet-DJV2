using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Gaïard : BasicEnemyBehaviour
{
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private float idealDistance;
    [SerializeField] private float aimChance;
    [SerializeField] private float changeDirectionFrequency;
    [SerializeField] private float attackFrequency;
    private float rotationDir = 1;
    // Start is called before the first frame update
    void Start()
    {   
        navMeshAgent = GetComponent<NavMeshAgent>();
        _target = PlayerController.Instance.transform;
        portalFlag = false;
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
            var l1 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l1.transform.SetParent(transform);
            l1.GetComponent<GaïardLeaf>().SetLeaf(3f, 1.5f * Vector3.up + Random.Range(-0.15f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, true, _target, 0);
            yield return new WaitForSeconds(0.1f);
            var l2 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l2.transform.SetParent(transform);
            l2.GetComponent<GaïardLeaf>().SetLeaf(3.4f, 1.5f * Vector3.up + Random.Range(-0.15f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, true, _target, 0);
            yield return new WaitForSeconds(0.1f);
            var l3 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l3.transform.SetParent(transform);
            l3.GetComponent<GaïardLeaf>().SetLeaf(3.8f, 1.5f * Vector3.up + Random.Range(-0.15f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, true, _target, 0);
        } 
        else 
        {
            var l1 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l1.transform.SetParent(transform);
            l1.GetComponent<GaïardLeaf>().SetLeaf(3f, 1.5f * Vector3.up + Random.Range(-0.35f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, false, _target, -30);
            yield return new WaitForSeconds(0.1f);
            var l2 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l2.transform.SetParent(transform);
            l2.GetComponent<GaïardLeaf>().SetLeaf(2.9f, 1.5f * Vector3.up + Random.Range(-0.15f,0.15f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, false, _target, 0);
            yield return new WaitForSeconds(0.1f);
            var l3 = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
            l3.transform.SetParent(transform);
            l3.GetComponent<GaïardLeaf>().SetLeaf(2.8f, 1.5f * Vector3.up + Random.Range(-0.15f,0.35f) * Vector3.right + Random.Range(-0.1f,0.1f) * Vector3.forward, false, _target, 30);
        }
        StartCoroutine(leafAttack());
    }
}
