using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DistanceEnemyBehaviour : MonoBehaviour
{
    private List<Material> _materials = new List<Material>();
    private List<Color> _initMaterialsColor = new List<Color>();
    [SerializeField] private AnimationCurve curve;

    [SerializeField] private GameObject _bulletPrefab;

    public float _cooldown = 2f;
    public float _cooldownTimer = 0f;
    
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
            if(_cooldownTimer >= _cooldown)
            {
                Shoot();
                _cooldownTimer = 0f;
            }
            else _cooldownTimer += Time.deltaTime;
        }

    }

    private void Shoot()
    {
        if (portalFlag)
        {
            return;
        }
        
        GameObject bullet = Instantiate(_bulletPrefab, transform.position + transform.forward*1.5f, transform.rotation);
        bullet.transform.position = new Vector3(bullet.transform.position.x, 1f, bullet.transform.position.z);
        bullet.GetComponent<Bullet>().target = _target;
        bullet.gameObject.SetActive(true);
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
