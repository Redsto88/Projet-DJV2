using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    [SerializeField] private new Camera camera;
   [SerializeField] private GameObject portalAim;
    [SerializeField] private GameObject portalPrefab;

    private Vector3 _portalTargetPosition;
    private bool _canSpawnPortal;

    [SerializeField] private int nbMaxPortal;
    private int _nbPortal;
    private List<Portal> _portals = new List<Portal>();

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var x))
        {
            _portalTargetPosition = x.point;

            if (x.collider.gameObject.layer == 3)
            {
                _canSpawnPortal = true;
                portalAim.transform.position = _portalTargetPosition;
                if (!portalAim.activeSelf)
                {
                    portalAim.SetActive(true);
                }
            }
            else
            {
                _canSpawnPortal = false;
                if(portalAim.activeSelf)
                {
                    portalAim.SetActive(false);
                }
            }
        }
    }
    

    public void CreatePortal()
    {
        if (_canSpawnPortal && _nbPortal < nbMaxPortal)
        {
            _nbPortal += 1;

            var portal = Instantiate(portalPrefab, _portalTargetPosition, Quaternion.identity);
            portal.gameObject.SetActive(true);
            _portals.Add(portal.GetComponent<Portal>());

            //On link chaque paire de portails entre eux
            if (_nbPortal != 0 && _nbPortal % 2 == 0)
            {
                _portals[_nbPortal - 1].linkPortal = _portals[_nbPortal - 2];
                _portals[_nbPortal - 2].linkPortal = _portals[_nbPortal - 1];
            }
        }
    }
    

    public void DeletePortals()
    {
        // On supprime tous les portails et on vide la mémoire
        foreach (var portal in _portals)
        {
            StartCoroutine(DestroyPortalCoroutine(portal.gameObject));
        }
        _nbPortal = 0;
        _portals.Clear();
    }

    public void DisablePortals()
    {
        foreach (var portal in _portals)
        {
            portal.gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator DestroyPortalCoroutine(GameObject go)
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(go);
    }
    
}