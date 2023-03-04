using Unity.VisualScripting;
using UnityEngine;

public class SphereDetector : MonoBehaviour
{
    public bool isActivated = false;
    public Material material;
    public Color color;
    public Color lightColor;

    public Light light;

    private void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        color = material.color;
        lightColor = light.color;
        if (!light.IsUnityNull())
        {
            light.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            sphere.detector = this;
            //change color to cyan
            material.color = Color.cyan * 5f;
            light.color = Color.cyan;
            
            isActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to red
            if (material.color == Color.green) return;
            material.color = color * 5;
            light.color = lightColor;
            isActivated = false;

        }
    }
}