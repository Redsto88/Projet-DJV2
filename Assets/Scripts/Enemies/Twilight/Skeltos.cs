using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeltos : BasicEnemyBehaviour
{
    [Header("Air movements settings")] 
    [SerializeField] private float movingSpeed;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float safeBackDistance;
    [SerializeField] private float baseHeight;
    [SerializeField] private float changeDirFrequency;
    [SerializeField] private int maxStuns;
    [Header("Ground movement settings")]
    [SerializeField] private float runningSpeed;
    [Header("Attack settings")] 
    [SerializeField] private float damage;
    [SerializeField] private float attackFrequency;
    [SerializeField] private float maxDetectionTime;
    [SerializeField] private float stunTime;
    [SerializeField] private AnimationCurve heightCurve;

    private float rotationDir;
    private int notAFool;
    private bool falling;
    private bool patienceAttack;
    private bool hasHit;
    private bool hasStunned;
    private bool hasEscapedPortal = false;
    private bool rushing;
    private Coroutine behave;
    private Coroutine startAir;
    private Coroutine currentMove;
    private float speedMod = 1;
    private Rigidbody rb;
    private Collider col;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Destroy(navMeshAgent);
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        startAir = StartCoroutine(start_air());
        StartCoroutine(changeDirection());
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!falling && startAir == null && !hasStunned && !patienceAttack && !rushing)
        {
            transform.LookAt(_target);
            var dir = rotationDir * transform.right;
            var offset = _target.position - transform.position;
            if (currentMove == null) dir += transform.forward * (offset.magnitude - safeBackDistance);
            dir = dir.normalized;
            Debug.DrawRay(transform.position, movingSpeed/3f*dir, Color.green, 0.5f);
            if (Physics.Raycast(transform.position, dir, movingSpeed/3f, LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal)) {if (speedMod > 0.05f) speedMod /= 1.1f;}
            else speedMod *= 1.1f;
            speedMod = Mathf.Clamp(speedMod,0,1);
            rb.MovePosition(transform.position + Time.fixedDeltaTime * ((speedMod < 0.05f) ? 0 : speedMod) * movingSpeed * dir);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            rb.constraints = (RigidbodyConstraints)((int)RigidbodyConstraints.FreezePositionY + (int)RigidbodyConstraints.FreezeRotationX + (int)RigidbodyConstraints.FreezeRotationZ);
        }
        else 
        {
            if (rushing)
            {
                transform.LookAt(_target);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            } 
            rb.constraints = RigidbodyConstraints.None;
        }
        col.isTrigger = falling || patienceAttack || currentMove != null;
        // rb.isKinematic = !falling;
        // rb.useGravity = !falling;
    }

    IEnumerator changeDirection()
    {
        var olddir = rotationDir;
        var newdir = Random.Range(0,2) * 2 - 1;
        var timeEllapsed = 0f;
        while (timeEllapsed < 1f)
        {
            rotationDir = Mathf.Lerp(olddir, newdir, timeEllapsed);
            timeEllapsed += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        rotationDir = newdir;
        yield return new WaitForSeconds(changeDirFrequency);
        StartCoroutine(changeDirection());
    }

    IEnumerator start_air()
    {
        var dir = new Vector3(Random.Range(0f,1f),0,Random.Range(0f,1f)).normalized;
        var timeEllapsed2 = 0f;
        while (timeEllapsed2 < 2)
        {
            rb.MovePosition(new Vector3(transform.position.x, Mathf.Lerp(0, baseHeight, timeEllapsed2/2f), transform.position.z));
            // transform.position = new Vector3(transform.position.x, Mathf.Lerp(0, baseHeight, timeEllapsed2/2f), transform.position.z);
            timeEllapsed2 += Time.deltaTime;
            yield return null;
        }
        startAir = null;
        behave = StartCoroutine(air_behaviour());
    }

    IEnumerator air_behaviour()
    {
        if (currentMove != null) yield break;
        yield return new WaitForSeconds(attackFrequency);
        if (currentMove != null) yield break;
        var patience = 0f;
        Debug.DrawRay(_target.position, safeBackDistance * transform.forward, Color.blue, 5f);
        //Raycast qui s'assure que le Skeltos ne se prendra pas le mur directement sur une attaque plongée
        while ((Physics.Raycast(_target.position, transform.forward, safeBackDistance, LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal) || Physics.Raycast(transform.position, _target.position - transform.position, Vector3.Distance(transform.position, _target.position), LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal)) && patience < maxDetectionTime)
        {
            Debug.DrawRay(_target.position, safeBackDistance * transform.forward, Color.blue, 5f);
            patience += Time.deltaTime;
            yield return null;
        }
        if (patience >= maxDetectionTime) //Attaque rapide inévitable si le joueur reste dans un coin
        {
            var basePos = transform.position;
            var timeEllapsed = 0f;
            patienceAttack = true;
            while (timeEllapsed < 3.1f)
            {
                if (timeEllapsed < 0.5f)
                {
                    rb.MovePosition(Vector3.Lerp(basePos, _target.position, timeEllapsed*2f));
                }
                else
                {
                    if (timeEllapsed > 0.6f)
                    rb.MovePosition(Vector3.Lerp(_target.position, basePos, (timeEllapsed - 0.6f)*2f/5f));
                }
                timeEllapsed += Time.fixedDeltaTime;
                yield return null;
            }
            patienceAttack = false;
            hasHit = false;
        }
        else //Sinon, attaque plongée qui peut rendre vulnérable
        {
            falling = true;
            var basePos = transform.position;
            var dest = _target.position + baseHeight * Vector3.up + transform.forward * safeBackDistance;
            var dist = Vector3.Distance(transform.position - transform.position.y * Vector3.up, dest);
            var fallDir = (dest - basePos).normalized;
            var timeEllapsed = 0f;
            var timeTotal = dist/fallingSpeed;
            while (timeEllapsed < timeTotal)
            {
                rb.MovePosition(new Vector3(transform.position.x, heightCurve.Evaluate(timeEllapsed/timeTotal), transform.position.z) + 0.8f * fallingSpeed * Time.fixedDeltaTime * transform.forward);
                // transform.position = new Vector3(transform.position.x, heightCurve.Evaluate(timeEllapsed/timeTotal), transform.position.z) + fallingSpeed * Time.fixedDeltaTime * transform.forward;
                timeEllapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            falling = false;
            hasHit = false;
        }
        StartCoroutine(air_behaviour()); //Relancer
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasStunned || currentMove != null) return;
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if (!hasHit)
            {
                player.GetComponent<PlayerManager>().ApplyDamage(damage);
                hasHit = true;
            }
        }
        else 
        {
            if (patienceAttack || other.gameObject.layer != 6) return; // 6 is Wall
            StopCoroutine(behave);
            falling = false;
            StartCoroutine(stunned(other.transform.up));
        }
    }

    IEnumerator run_in()
    {
        rushing = false;
        if (!hasEscapedPortal) yield return new WaitForSeconds(Random.Range(0f,2f));
        rushing = true;
        Debug.Log("starting rush");
        while (Vector3.Distance(transform.position, _target.position) > 1) //run on player
        {
            var dir = (_target.position - transform.position).normalized;
            rb.MovePosition(transform.position + runningSpeed * Time.fixedDeltaTime * dir);
            Debug.DrawRay(transform.position + Vector3.up, transform.forward * runningSpeed/5f, Color.cyan, 5f);
            Debug.Log("HasEscapedPortal ? " + hasEscapedPortal);
            var portal = Physics.Raycast(transform.position + Vector3.up, transform.forward, runningSpeed/3f, LayerMask.GetMask("Portal"), QueryTriggerInteraction.UseGlobal);
            var wall = Physics.Raycast(transform.position, _target.position - transform.position, Vector3.Distance(transform.position, _target.position), LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal);
            if (portal || wall)
            {
                Debug.Log("PORTAL OR WALL AHEAD");
                if (wall || !hasEscapedPortal)
                {
                    if (Physics.Raycast(transform.position, transform.right, 0.75f * runningSpeed, LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal)) currentMove = StartCoroutine(dash_dir(0));
                    else 
                    {
                        if (Physics.Raycast(transform.position, -transform.right, 0.75f * runningSpeed, LayerMask.GetMask("Wall"), QueryTriggerInteraction.UseGlobal)) currentMove = StartCoroutine(dash_dir(1));
                        else currentMove = StartCoroutine(dash_dir(Random.Range(0,2)));
                    }
                    if (portal) hasEscapedPortal = true;
                    yield break;
                }
            }
            yield return new WaitForFixedUpdate();
        }
        hasEscapedPortal = false;
        // TODO launch attack anim
        currentMove = StartCoroutine(run_in());
    }

    IEnumerator dash_dir(int dir)
    {
        var jumpDir = (dir * 2 - 1) * transform.right;
        var timeEllapsed = 0f;
        while (timeEllapsed < 0.4f)
        {
            rb.MovePosition(transform.position + Time.fixedDeltaTime * runningSpeed * 2 * jumpDir);
            timeEllapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        currentMove = StartCoroutine(run_in());
    }


    IEnumerator stunned(Vector3 projDir)
    {
        hasStunned = true;
        notAFool++;
        var timeEllapsed = 0f;
        var fallPos = transform.position + 2 * projDir;
        var pos = transform.position;
        Debug.DrawRay(transform.position, + 2*projDir, Color.red, Mathf.Infinity);
        while (timeEllapsed < 1f)
        {
            transform.position = Vector3.Lerp(pos, new Vector3(fallPos.x, 0.1f, fallPos.z), timeEllapsed);
            timeEllapsed += Time.fixedDeltaTime;
            yield return null;
        }
        rb.MovePosition(new Vector3(fallPos.x, 0.1f, fallPos.z));
        yield return new WaitForSeconds(stunTime);
        if (notAFool >= maxStuns)
        {
            Debug.Log("ah");
            StopCoroutine(behave);
            currentMove = StartCoroutine(run_in());
        }
        else
        {
            Debug.Log("start flying again");
            startAir = StartCoroutine(start_air());
            while (behave == null) yield return new WaitForEndOfFrame();
            hasStunned = false;
        }
    }
}
