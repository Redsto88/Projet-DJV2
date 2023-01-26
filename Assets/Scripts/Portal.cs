using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public Portal linkPortal;
<<<<<<< Updated upstream
    public GameObject aim;

=======
    [SerializeField] private GameObject aim;
    public Transform transitionFwd;
    public Transform transitionBwd;
    private Transform _destinationTransition;
    private Vector3 _velocity;
    
>>>>>>> Stashed changes
    private void OnEnable()
    {
        StartCoroutine(OrientationCoroutine());
    }

    IEnumerator OrientationCoroutine()
    {
        while (Input.GetButton("Portal"))
        {
            transform.LookAt(aim.transform.position);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!linkPortal.IsUnityNull())
        {
           StartCoroutine(Teleport(other));
        }
    }

<<<<<<< Updated upstream
    private void OnTriggerStay(Collider other)
    {
        print("portal trigger stay");
    }

    private IEnumerator Teleport(Collider collider)
=======
    private IEnumerator Teleport(Collider col)
>>>>>>> Stashed changes
    {
        linkPortal.GetComponent<BoxCollider>().enabled = false;

        //Cas du joueur
        if (col == PlayerController.Instance.characterController)
        {
            //On cherche le sens d'orientation 
            if (Vector3.Dot(PlayerController.Instance.playerPivot.transform.forward, transform.forward) > 0)
            {
                _destinationTransition = linkPortal.transitionBwd; //On rentre par le côté "vert" du portail (Arrière), on doit ressortir du côté vert
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
                PlayerController.Instance.transform.position = Vector3.SmoothDamp(PlayerController.Instance.transform.position, 
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
            
            NavMeshAgent agent = enemy.navMeshAgent;
            float stopDistance = agent.stoppingDistance;
            
            //On désactive l'agent pour la téléportation
            enemy.portalFlag = true;
            agent.enabled = false;

            //Téléportation
            col.gameObject.transform.position = linkPortal.transform.position;
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
        else
        {
            col.gameObject.transform.position = linkPortal.transform.position;
            col.gameObject.transform.rotation = linkPortal.transform.rotation;
            yield return new WaitForEndOfFrame();
        }

        
        PlayerController.Instance.gameObject.GetComponent<SpawnPortal>().DeletePortals();
        
    }

    
}
