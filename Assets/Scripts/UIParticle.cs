using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIParticle : MonoBehaviour
{
    private float LifeTime;
    private Vector2 speed;
    private Vector2 acceleration;
    private Vector2 sizeVariation;
    private float rotSpeed;
    private float rotAcceleration;
    private float timeEllapsed;
    private Transform center;
    private float strength;
    private RectTransform rt;
    private Image img;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeEllapsed > LifeTime || transform.localScale.x < 0 || transform.localScale.y < 0) Destroy(gameObject);
        if (center != null) 
        {
            var dir = new Vector2(transform.position.x - center.transform.position.x, transform.position.y - center.transform.position.y).normalized;
            rt.anchorMin += strength * Time.deltaTime * dir;
            rt.anchorMax += strength * Time.deltaTime * dir;
        }
        timeEllapsed += Time.deltaTime;
        rt.anchorMin += speed * Time.deltaTime;
        rt.anchorMax += speed * Time.deltaTime;
        speed -= acceleration * Time.deltaTime;
        transform.localEulerAngles += rotSpeed * Time.deltaTime * Vector3.forward;
        rotSpeed += rotAcceleration * Time.deltaTime;
        transform.localScale += Time.deltaTime * new Vector3(sizeVariation.x, sizeVariation.y, 0);
    }

    public void SetParticle(float lt, Vector2 s, Vector2 a, Vector2 sV, float angle, float angleS, float angleA, bool pulse, Transform c, float str)
    {
        LifeTime = lt;
        speed = s;
        acceleration = a;
        sizeVariation = sV;
        transform.localEulerAngles = angle * Vector3.forward;
        rotSpeed = angleS;
        rotAcceleration = angleA;
        if (pulse)
        {
            center = c;
            strength = str;
        }
        else 
        {
            center = null;
            strength = 0;
        }
        img.SetNativeSize();
        img.preserveAspect = true;
    }
}
