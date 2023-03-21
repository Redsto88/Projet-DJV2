using Unity.VisualScripting;
using UnityEngine;

public class SphereDetector : MonoBehaviour
{
    public bool isActivated = false;
    public Material material;
    public Color color;
    public Color lightColor;

    public new Light light;

    private void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        color = material.color;
        lightColor = light.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            sphere.detector = this;
            //change color to cyan
            AudioManager.Instance.PlaySFX("Enigme_Interrupteur");
            material.color = Color.cyan * 5f;
            light.color = Color.cyan;
            
            isActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<SphereEnigme>(out var sphere))
        {
            //change color to baseColor
            if (light.color != Color.green)
            { 
                AudioManager.Instance.PlaySFX("Enigme_Fail");
                material.color = color * 5;
                light.color = lightColor;
                isActivated = false;
            }
        }
    }
}