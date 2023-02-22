using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyBehaviour : IDamageable
{
    private List<Material> _materials = new List<Material>();
    private List<Color> _initMaterialsColor = new List<Color>();
    [SerializeField] private AnimationCurve curve;

    public bool isTest = false;
    
    [Header("Stats")]

    public NavMeshAgent navMeshAgent;
    protected Transform _target;
    
    public bool portalFlag;
    public bool attackFlag;
    
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        foreach (var render in renderer)
        {
            foreach (var mat in render.materials)
            {
                _materials.Add(mat);
                _initMaterialsColor.Add(mat.color);
            }
        }
        
        _health = healthMax;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        _target = PlayerController.Instance.transform;

        _animator = GetComponentInChildren<Animator>();
        
        portalFlag = false;
        attackFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(navMeshAgent.path.status);
        for (int i = 0; i < navMeshAgent.path.corners.Length - 1; i++)
            Debug.DrawLine(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1], Color.red);
        
        if (!_target.IsUnityNull())
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                if (navMeshAgent.path.status == NavMeshPathStatus.PathComplete && !portalFlag && !attackFlag)
                {
                    print("avance");
                    _animator.SetBool(IsWalking, true);
                    navMeshAgent.destination = _target.position;
                }
                else
                {
                    print("idle");
                    _animator.SetBool(IsWalking, false);
                    navMeshAgent.destination = transform.position;
                }
            }
            else
            {
                navMeshAgent.destination = transform.position;
                Attack();
            }
        }
        
        else if (_target.IsUnityNull() || navMeshAgent.path.status != NavMeshPathStatus.PathComplete)
        {
            _animator.SetBool(IsWalking, false);
            navMeshAgent.destination = transform.position;
        }
    }

    private void Attack()
    {
        if (!attackFlag)
        {
            attackFlag = true;
            _animator.CrossFade("Attack_01", 0.1f);
        }
    }


    // IDamageable
    public override void ApplyDamaged(float damage)
    {
        StartCoroutine(ColorCoroutine());
        base.ApplyDamaged(damage);
        if (_health <= 0) RoomBehaviour.Instance.CountEnemyDeath();
    }

    IEnumerator ColorCoroutine()
    {
        const float duration = 0.5f;
        var timeLeft = duration;

        while (timeLeft > 0f)
        {
            var lerpValue = timeLeft > duration / 2f
                ? 2f * (1f - timeLeft / duration)
                : 2f * timeLeft / duration;


            for (var i = 0; i < _materials.Count; i += 1)
            {
                var material = _materials[i];

                material.color = Color.Lerp(_initMaterialsColor[i], 3*Color.white, curve.Evaluate(duration - timeLeft));
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
