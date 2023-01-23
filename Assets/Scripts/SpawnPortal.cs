using System.Collections;
using System.Collections.Generic;
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
    public List<GameObject> _portals;
    
    // Update is called once per frame
    void Update()
    {
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        
        //var plane = new Plane(Vector3.up, Vector3.zero);
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
            _portals.Add(portal);
        }
    }

    public void DeletePortals()
    {
        foreach (var portal in _portals)
        {
            Destroy(portal);
        }
        _nbPortal = 0;
        _portals.Clear();
    }
    
}