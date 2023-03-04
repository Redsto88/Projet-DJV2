using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    public Transform start;

    public float force = 5f;

    [SerializeField] private GameObject spherePrefab;

    private GameObject sphere;


    // Start is called before the first frame update
   /* void Start()
    {
        sphere = Instantiate(spherePrefab, start.position, start.rotation);
        sphere.GetComponent<SphereEnigme>().speed = speed;
    }
*/
    // Update is called once per frame
    void Update()
    {
        if(sphere == null)
        {
            sphere = Instantiate(spherePrefab, start.position, start.rotation);
            sphere.GetComponent<SphereEnigme>().speed = force;
            sphere.GetComponent<Rigidbody>().AddForce(transform.right * force, ForceMode.Impulse);
            //sphere.transform.position = start.position;
            //sphere.transform.rotation = start.rotation;
        }
    }
}
