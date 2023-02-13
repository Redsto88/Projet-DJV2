using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Coroutine move;
    [SerializeField] private float moveTime;
    [SerializeField] private Vector2 shownPosMin;
    [SerializeField] private Vector2 shownPosMax;
    [SerializeField] private Vector2 hiddenPosMin;
    [SerializeField] private Vector2 hiddenPosMax;
    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData p)
    {
        if (move != null) StopCoroutine(move);
        move = StartCoroutine(show());
        Debug.Log("entered");
    }

    IEnumerator show()
    {
        var timeEllapsed = 0f;
        var currentMin = rt.anchorMin;
        var currentMax = rt.anchorMax;
        while (timeEllapsed < moveTime)
        {
            rt.anchorMin = Vector2.Lerp(currentMin, shownPosMin, timeEllapsed/moveTime);
            rt.anchorMax = Vector2.Lerp(currentMax, shownPosMax, timeEllapsed/moveTime);
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void OnPointerExit(PointerEventData p)
    {
        if (move != null) StopCoroutine(move);
        move = StartCoroutine(hide());
        Debug.Log("exited");
    }

    IEnumerator hide()
    {
        var timeEllapsed = 0f;
        var currentMin = rt.anchorMin;
        var currentMax = rt.anchorMax;
        while (timeEllapsed < moveTime)
        {
            rt.anchorMin = Vector2.Lerp(currentMin, hiddenPosMin, timeEllapsed/moveTime);
            rt.anchorMax = Vector2.Lerp(currentMax, hiddenPosMax, timeEllapsed/moveTime);
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
