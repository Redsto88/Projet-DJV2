using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAutoBuilder : MonoBehaviour
{
    protected void Awake()
    {
        NavMesh.RemoveAllNavMeshData();
        
        var markups = new List<NavMeshBuildMarkup>();
        var sources = new List<NavMeshBuildSource>();


        var markup = new NavMeshBuildMarkup();
        markup.root = transform;
        markup.overrideArea = true;
        markup.area = NavMesh.GetAreaFromName("Walkable");

        NavMeshBuilder.CollectSources(transform,
            LayerMask.GetMask("Ground"),
            NavMeshCollectGeometry.PhysicsColliders,
            NavMesh.GetAreaFromName("Walkable"),
            markups,
            sources);
        
        var settings = NavMesh.GetSettingsByIndex(0);
        var settings2 = new NavMeshBuildSettings();
        settings2.agentTypeID = 0;
        settings2.agentRadius = 0.5f;
        settings2.agentHeight = 2f;
        settings2.agentSlope = 45f;
        settings2.agentClimb = 4f;


        var data = NavMeshBuilder.BuildNavMeshData(
            settings2,
            sources,
            new Bounds(
                Vector3.zero,
                new Vector3(5000f, 5000f, 5000f)),
            transform.position,
            Quaternion.identity);

        NavMesh.AddNavMeshData(data);
    }
        
        
}
