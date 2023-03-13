using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float slowdownFactor = 0.05f;
    public float slowdownEndTransitionLength = 2f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void StopSlowMotion()
    {
        StartCoroutine(StopSlowMotionCoroutine());
        PlayerManager.Instance.isFocused = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    IEnumerator StopSlowMotionCoroutine()
    {
        while (1 - Time.timeScale > 0.01)
        {
            Time.timeScale += (1f / slowdownEndTransitionLength) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            yield return null;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
