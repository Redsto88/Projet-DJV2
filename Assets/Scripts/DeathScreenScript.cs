using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class DeathScreenScript : MonoBehaviour
{

    [SerializeField] private Image screen;
    [SerializeField] private RectTransform text;
    [SerializeField] private float fadeTime = 1f;

    private void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        screen.color = new Color(0, 0, 0, 0);
        text.anchoredPosition = new Vector2(0, 3000);
        float time = 0;
        while (time < fadeTime)
        {
            time += Time.deltaTime;
            screen.color = new Color(0, 0, 0, time);
            yield return null;
        }
        float time2 = 0;
        while (time2 < fadeTime)
        {
            time2 += Time.deltaTime;
            text.anchoredPosition = new Vector2(Mathf.Lerp(1500, 0, time2) , 0);
            yield return null;
        }

    }
}
