using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class BossBehaviour : BasicEnemyBehaviour
{
    [Header("Mouvements")]
    public float stoppingDistance = 12f;
    public float toCloseDistance = 5f;

    private bool canBeHit = false;

    private bool inJump = false;

    private bool isStunned = false;

    public bool phase2 = false;
    public List<PlateformeBoss> plateformeBoss;

    [Header("Attaques")]
    [SerializeField] private float leafAttackChance;
    [SerializeField] private  int leafNumber = 5;
    [SerializeField] private float leafDamage = 15f;

    [SerializeField] private float sphereForce;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject animationSphere;
    [SerializeField] private Transform sphereSpawnPoint;
    [SerializeField] private float aimChance;
    [SerializeField] private float changeDirectionFrequency;
   // private float attackTimer = 0f;
    [SerializeField] private GameObject leafPrefab;

    [SerializeField] private List<PlateformeBoss> plateformes = new List<PlateformeBoss>();

    [Header("Autres")] 
    [SerializeField] private UIHealthBar healthBar;
    [SerializeField] private GameObject solP1;
    [SerializeField] private GameObject solP2;
    [SerializeField] private SphereDetector sphereDetector;
    [SerializeField] private Transform boosTransformStartP2;
    [SerializeField] private Transform playerTransformStartP2;
    [SerializeField] private GameObject bouleAttaquePS;
    [SerializeField] private Canvas endCanva;
   
    
    [Header("Room")]
    [SerializeField] private BasicEnemyBehaviour enemy;

    [SerializeField] private RoomBehaviour room;
    
    
    private bool _isAttacking;
    private float _speed;
    private static readonly int IsStunned = Animator.StringToHash("isStunned");
    

    // Update is called once per frame
    protected override void Start()
    {
        if (room.ennemiesActiveAtStartup)
        {

            base.Start();
            solP2.SetActive(false);
            _isAttacking = true;
            _speed = navMeshAgent.speed;
            StartCoroutine(FirstCoroutine());
            AudioManager.Instance.PlayMusic("Boss_Intro");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator FirstCoroutine()
    {
        yield return new WaitForSeconds(4f);
        _isAttacking = false;
    }


    void Update()
    {
        if (!portalFlag && !_target.IsUnityNull() && !isStunned)
        {
            Vector3 targetDir = _target.position - transform.position;
            if(!inJump){
                navMeshAgent.destination = _target.position - targetDir.normalized*stoppingDistance;
                if((transform.position - _target.position).magnitude < toCloseDistance)
                {
                    inJump = true;
                    GoAway();
                }
            }
            float step = 5 * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            if(!_isAttacking)
            {
                Attack();
                //attackTimer = 0f;
            }
            //else attackTimer += Time.deltaTime;
        }

        if(sphereDetector.isActivated && !isStunned)
        {
            print("STUNNED");
            canBeHit = true;
            isStunned = true;
            animator.SetBool(IsStunned,true);
            navMeshAgent.enabled = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            StartCoroutine(Stun());
        }

        if(!sphereDetector.isActivated && isStunned)
        {
            print("UNSTUNNED");
            canBeHit = false;
            isStunned = false;
            animator.SetBool(IsStunned,false);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            navMeshAgent.enabled = true;
        }

        
    }

    IEnumerator Stun()
    {
        //animation ?
        yield return null;
    }


    private void GoAway()
    {
        print("GO AWAY");
        StartCoroutine(Jump());

        IEnumerator Jump()
        {
            Vector3 targetDir = _target.position - transform.position;
            navMeshAgent.destination = _target.position - targetDir.normalized*stoppingDistance;
            navMeshAgent.enabled = false;
            float jumpHeight = 7.5f;
            float jumpTime = 1f;
            Vector3 jumpTarget = _target.position + (toCloseDistance*2)*(_target.position - transform.position).normalized;
            Vector3 jumpStart = transform.position;
            float time = 0f;
            while (time < jumpTime)
            {
                time += Time.deltaTime;
                float y = jumpHeight * Mathf.Sin(Mathf.PI * time / jumpTime);
                transform.position = Vector3.Lerp(jumpStart, jumpTarget, time / jumpTime) + Vector3.up * y;
                yield return null;
            }
            navMeshAgent.enabled = true;
            inJump = false;
        }
    }

    public override void ApplyDamage(float damage)
    {
       if (canBeHit)
       {
            base.ApplyDamage(damage);
            healthBar.SetHealth(_health);
            if (_health < healthMax/2 && !phase2)
            {
                solP1.SetActive(false);
                solP2.SetActive(true);
                phase2 = true;
                foreach (var plateforme in plateformeBoss)
                {
                    plateforme.ToUp();
                    isStunned = false;
                    transform.position = boosTransformStartP2.position;
                    transform.rotation = boosTransformStartP2.rotation;
                    PlayerController.Instance.transform.position = playerTransformStartP2.position;
                    //PlayerController.Instance.transform.rotation = playerTransformStartP2.rotation;

                }
            } ;
       }
    }

    void Attack()
    {
        if(Random.Range(0f,1f) < leafAttackChance)
        {
            StartCoroutine(LeafAttack());
        }
        else
        {
             StartCoroutine(SphereAttack());
        }
    }

    private IEnumerator LeafAttack()
    {
        navMeshAgent.speed = _speed/2;
        _isAttacking = true;
        animator.CrossFade("PrepareAttack",0.2f);
        AudioManager.Instance.PlaySFX("LeafsCast");
        if (Random.Range(0f,1f) < aimChance)
        {
            for(int i = 0;i<leafNumber;i++)
            {
                yield return new WaitForSeconds(0.1f);
                var l = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
                l.transform.SetParent(transform);
                l.GetComponent<GaïardLeaf>().SetLeaf(3f + 0.4f * i,
                    1.5f * Vector3.up + Random.Range(-0.15f, 0.15f) * Vector3.right +
                    Random.Range(-0.1f, 0.1f) * Vector3.forward,
                    true,
                    _target,
                    0,
                    leafDamage,
                    animator,
                    (i % 2 == 0) ? "Attack_R" : "Attack_L",
                    "Boss_LeafAttack");
            }
            yield return new WaitForSeconds(3 + 0.4f * (leafNumber - 1));
        
        } 
        else 
        {
            for(int i=0;i<leafNumber;i++)
            {
                yield return new WaitForSeconds(0.1f);
                var l = Instantiate(leafPrefab, transform.position + Vector3.up, transform.rotation);
                l.transform.SetParent(transform);
                l.GetComponent<GaïardLeaf>().SetLeaf(3f - 0.1f * i,
                    1.5f * Vector3.up +
                    Random.Range(-0.35f + i / leafNumber * 0.7f, -0.35f + (i + 1) / leafNumber * 0.7f) * Vector3.right +
                    Random.Range(-0.1f, 0.1f) * Vector3.forward,
                    false,
                    _target,
                    -30 + 60 * i / (leafNumber - 1),
                    leafDamage,
                    animator,
                    "Attack_B",
                    "Boss_LeafAttack");
            }
            yield return new WaitForSeconds(3 - 0.1f * (leafNumber - 1));
        
        }
        navMeshAgent.speed = _speed;
        yield return new WaitForSeconds(Random.Range(coolDown - 2, coolDown + 2));
        _isAttacking = false;
    }

    private IEnumerator SphereAttack()
    {
        navMeshAgent.speed = 0;
        _isAttacking = true;
        animator.CrossFade("Attack_Boule",0.1f);
        AudioManager.Instance.PlaySFX("Boss_Sphere_Attack");
        float time = 0f;
        float growTime = 1f;
        var ps = Instantiate(bouleAttaquePS, sphereSpawnPoint.position, Quaternion.identity);
        animationSphere.SetActive(true);
        while(time < growTime)
        {
            time += Time.deltaTime;
            animationSphere.transform.localScale = Vector3.one * Mathf.Lerp(0f, 1/0.7f, time);
            yield return null;
        }
        animationSphere.SetActive(false);
        GameObject sphere = Instantiate(spherePrefab, sphereSpawnPoint.position, transform.rotation);
        sphere.GetComponent<SphereEnigme>().speed = sphereForce;
        sphere.GetComponent<Rigidbody>().AddForce((_target.position-sphereSpawnPoint.position).normalized * sphereForce, ForceMode.Impulse);

        navMeshAgent.speed = _speed;
        yield return new WaitForSeconds(Random.Range(coolDown - 2, coolDown + 2));
        _isAttacking = false;
    }

    private void OnDestroy()
    {
        endCanva.gameObject.SetActive(true);
        if (_health <= 0)
        {
            AudioManager.Instance.PlayMusic("Boss_Fin");
        }
        enemy.ApplyDamage(9999);
        AudioManager.Instance.PlaySFX("Boss_Death");
    }
}
