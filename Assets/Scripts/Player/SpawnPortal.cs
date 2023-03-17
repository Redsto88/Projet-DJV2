using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    [SerializeField] private PortalCursor portalCursor;
    [SerializeField] private GameObject portalPrefab;
    
    private bool _canSpawnPortal;

    [SerializeField] private int nbMaxPortal;
    private int _nbPortal;
    private List<Portal> _portals = new List<Portal>();
    
    private ParticleSystem.EmissionModule _emissionModule1;
    private ParticleSystem.EmissionModule _emissionModule2;
    private ParticleSystem.EmissionModule _emissionModule3;
    private ParticleSystem.EmissionModule _emissionModule4;
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");


    private void Update()
    {
        _canSpawnPortal = portalCursor.canSpawnPortal;
    }

    public void CreatePortal()
    {
        if (_canSpawnPortal && _nbPortal < nbMaxPortal)
        {
            _nbPortal += 1;
            var portal = Instantiate(portalPrefab, portalCursor.transform.position, Quaternion.identity);
            portal.GetComponent<Portal>().portalCursor = portalCursor;
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
            StartCoroutine(DestroyPortalCoroutine(portal));
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

    IEnumerator DestroyPortalCoroutine(Portal portal)
    {
        _emissionModule1 = portal.fwdParticules.emission;
        _emissionModule1.enabled = false;
        _emissionModule2 = portal.fwdBlackBeam.emission;
        _emissionModule2.enabled = false;
        _emissionModule3 = portal.bwdParticules.emission;
        _emissionModule3.enabled = false;
        _emissionModule4 = portal.bwdBlackBeam.emission;
        _emissionModule4.enabled = false;
        
        float timeElapsed = 0;
        float lerpDuration = 1f;
        while (timeElapsed < lerpDuration)
        {
            portal.fwdMaterial.SetFloat(DissolveAmount,Mathf.Lerp(2, 17, timeElapsed / lerpDuration));
            portal.bwdMaterial.SetFloat(DissolveAmount, Mathf.Lerp(2, 17, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        portal.fwdMaterial.SetFloat(DissolveAmount,17);
        portal.bwdMaterial.SetFloat(DissolveAmount,17);

        
        
        Destroy(portal.gameObject);
    }
    
}