using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public Portal linkPortal;
    public PortalCursor portalCursor;

    [Header("Use in destruction")] 
    public Material fwdMaterial;
    [SerializeField] private GameObject fwdPortal;
    public ParticleSystem fwdParticules;
    public ParticleSystem fwdBlackBeam;
    public Material bwdMaterial;
    [SerializeField] private GameObject bwdPortal;
    public ParticleSystem bwdParticules;
    public ParticleSystem bwdBlackBeam;

    public Transform transitionFwd;
    public Transform transitionBwd;
    private Transform _destinationTransition;
    private Vector3 _velocity;
    private bool _isSpawned;

    public bool IsSpawned => _isSpawned;
    private void OnEnable()
    {
        fwdMaterial = fwdPortal.GetComponent<Renderer>().material;
        bwdMaterial = bwdPortal.GetComponent<Renderer>().material;
        StartCoroutine(OrientationCoroutine());
    }

    IEnumerator OrientationCoroutine()
    {
        print(portalCursor);
        _isSpawned = false;
        TimeManager.Instance.DoSlowMotion();
        portalCursor.cursorSpeed *= 5;
        while (Input.GetButton("Portal"))
        {
            transform.LookAt(new Vector3(portalCursor.transform.position.x, transform.position.y, portalCursor.transform.position.z));
            yield return null;
        }
        TimeManager.Instance.StopSlowMotion();
        portalCursor.cursorSpeed /= 5;
        _isSpawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!linkPortal.IsUnityNull())
        {
           StartCoroutine(Teleport(other));
        }
    }
    
    private IEnumerator Teleport(Collider col)
    {
        if (_isSpawned && linkPortal.IsSpawned) // On vérifie que les deux portails sont bien placés (plus dans le bullet time)
        {
            linkPortal.GetComponent<BoxCollider>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            //Cas du joueur
            if (col == PlayerController.Instance.characterController)
            {
                //On cherche le sens d'orientation 
                if (Vector3.Dot(PlayerController.Instance.playerPivot.transform.forward, transform.forward) > 0)
                {
                    _destinationTransition =
                        linkPortal
                            .transitionBwd; //On rentre par le côté "vert" du portail (Arrière), on doit ressortir du côté vert
                }
                else
                {
                    _destinationTransition = linkPortal.transitionFwd; //Idem mais pour l'avant
                }

                //On désactive le character controller pour se téléporter. IL ne pourra pas prendre de dégats pendant ce temps
                col.enabled = false;
                PlayerController.Instance.portalFlag = true;

                //Téléportation
                col.gameObject.transform.position = linkPortal.transform.position;
                PlayerController.Instance.playerPivot.transform.LookAt(_destinationTransition.position);
                yield return new WaitForEndOfFrame();

                //Animation portail
                /*foreach (var VARIABLE in  GetComponentsInChildren<Renderer>())
                {
                    VARIABLE.enabled = false;
                }
                foreach (var VARIABLE in  linkPortal.GetComponentsInChildren<Renderer>())
                {
                    VARIABLE.enabled = false;
                }*/

                //Transition
                while ((PlayerController.Instance.transform.position - _destinationTransition.position).magnitude > 0.1)
                {
                    PlayerController.Instance.transform.position = Vector3.SmoothDamp(
                        PlayerController.Instance.transform.position,
                        _destinationTransition.position, ref _velocity, 0.2f);
                    yield return null;
                }

                //On rend le controle au joueur
                col.enabled = true;
                PlayerController.Instance.portalFlag = false;
            }

            //Cas d'un ennemi
            else if (col.TryGetComponent(out BasicEnemyBehaviour enemy))
            {
                //On cherche le sens d'orientation 
                if (Vector3.Dot(col.transform.forward, transform.forward) > 0)
                {
                    _destinationTransition = linkPortal.transitionBwd; //On rentre par le côté "vert" du portail (Arrière), on doit ressortir du côté vert
                }
                else
                {
                    _destinationTransition = linkPortal.transitionFwd; //Idem mais pour l'avant
                }
                
                if (!enemy.TryGetComponent<NavMeshAgent>(out var agent))
                {
                    enemy.transform.position = _destinationTransition.position;
                    enemy.transform.rotation = _destinationTransition.rotation;
                }
                else 
                {
                    // NavMeshAgent agent = enemy.navMeshAgent;
                    float stopDistance = agent.stoppingDistance;
                    
                    //On désactive l'agent pour la téléportation
                    enemy.portalFlag = true;
                    agent.enabled = false;

                    //Téléportation
                    col.gameObject.transform.position = linkPortal.transform.position + new Vector3(0, 0.5f, 0);
                    col.transform.LookAt(_destinationTransition.position);
                    yield return new WaitForEndOfFrame();

                    //Animation portail
                    foreach (var VARIABLE in  GetComponentsInChildren<Renderer>())
                    {
                        VARIABLE.enabled = false;
                    }
                    foreach (var VARIABLE in  linkPortal.GetComponentsInChildren<Renderer>())
                    {
                        VARIABLE.enabled = false;
                    }

                    //Transition
                    while ((enemy.transform.position - _destinationTransition.position).magnitude > 0.1)
                    {
                        enemy.transform.position = Vector3.SmoothDamp(enemy.transform.position, 
                            _destinationTransition.position, ref _velocity, 0.2f);
                        yield return null;
                    }
                    
                    //On réactive l'agent
                    agent.enabled = true;
                    
                    //On remet l'ennemi en marche vers le joueur
                    agent.stoppingDistance = stopDistance;
                    enemy.portalFlag = false;
                }
            }
            
            //Cas d'une bullet
            else if(col.TryGetComponent(out AProjectile projectile))
            {
                projectile.gameObject.transform.position += linkPortal.transform.position - transform.position;
                if(Vector3.Dot(col.transform.forward, transform.forward) > 0){
                    projectile.gameObject.transform.rotation = Quaternion.Euler(0, linkPortal.transform.rotation.eulerAngles.y + 180, 0);
                }
                else
                {
                    projectile.gameObject.transform.rotation = linkPortal.transform.rotation;
                }
                yield return new WaitForEndOfFrame();
            }
            
            //Cas d'une sphere
            else if (col.TryGetComponent(out SphereEnigme sphere))
            {
                print("sphere");
                
                float tmpSpeed = sphere.GetComponent<Rigidbody>().velocity.magnitude;
                sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;

                sphere.transform.position += linkPortal.transform.position - transform.position;
                
                Vector3 orientation = new Vector3();
                if(Vector3.Dot(col.transform.forward, transform.forward) > 0)
                {
                    orientation = -linkPortal.transform.forward;
                }
                else
                {
                    orientation = linkPortal.transform.forward;
                }
                sphere.GetComponent<Rigidbody>().AddForce(tmpSpeed * orientation.normalized,ForceMode.Impulse);
                
            }
            //Cas par défaut
            else
            {
                print("defaut");
                projectile.gameObject.transform.position = linkPortal.transform.position;
                projectile.gameObject.transform.rotation = linkPortal.transform.rotation;
                yield return new WaitForEndOfFrame();
            }
            PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
        }
    }

    
}
