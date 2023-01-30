using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyBehaviour : MonoBehaviour, IDamageable
{
    private List<Material> _materials = new List<Material>();
    private List<Color> _initMaterialsColor = new List<Color>();
    [SerializeField] private AnimationCurve curve;
    
    [Header("Stats")] [SerializeField] 
    private float healthMax = 30;
    private float _health;

    public NavMeshAgent navMeshAgent;
    private Transform _target;

    public bool portalFlag;
    
   // public float speed;
   // public float minDist;
    // Start is called before the first frame update
    void Start()
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
        portalFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!portalFlag && !_target.IsUnityNull())
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                navMeshAgent.destination = _target.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
            }
        }
    }
    
    
    // IDamageable
    public void ApplyDamaged(float damage)
    {
        print("ennemi : apply damage");
        StartCoroutine(ColorCoroutine());
        _health -= damage;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetHealthMax()
    {
        return healthMax;
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

                material.color = Color.Lerp(_initMaterialsColor[i], Color.white, curve.Evaluate(duration - timeLeft));
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
