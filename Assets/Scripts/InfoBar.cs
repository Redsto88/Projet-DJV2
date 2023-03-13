using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoBar : MonoBehaviour
{
    [SerializeField] private bool isMouseButton;
    private bool isntMouseButton => !isMouseButton;
    [NaughtyAttributes.ShowIf("isMouseButton")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private int buttonInt;
    [NaughtyAttributes.ShowIf("isMouseButton")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private bool press = true;
    [NaughtyAttributes.ShowIf("isntMouseButton")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private KeyCode key;
    [SerializeField] private bool addCustomText;
    [NaughtyAttributes.ShowIf("addCustomText")]
    [NaughtyAttributes.AllowNesting]
    [SerializeField] private string customText;
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private string actionDesc;
    [SerializeField] private TextMeshProUGUI actionDescText;
    private Image background;
    private RectTransform rt;
    private float baseHeight;
    private bool isActive;
    private bool isValidated;
    private CanvasGroup cg;
    public int positionForAppearing; //TODO RETIRER CA C'EST HORRIBLE MAIS J'AI PLUS LE TEMPS ALED
    private Coroutine cor;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        background = transform.GetChild(0).GetComponent<Image>();
        if (addCustomText) keyText.text = customText;
        else 
        {
            if (isMouseButton)
            switch (buttonInt)
            {
                case 0: keyText.text = "Clic gauche"; break;
                case 1: keyText.text = "Clic droit"; break;
                case 2: keyText.text = "Clic molette"; break;
            }
            else
            keyText.text = key.ToString();
        }
        actionDescText.text = actionDesc;
        baseHeight = rt.rect.height;
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isValidated) return;
        if (!isActive) {if (transform.GetSiblingIndex() <= positionForAppearing && cor == null) ShowNewTask(); return;} //TODO SAME RETIRE
        if (isMouseButton)
        {
            if (press)
            {
                if (Input.GetMouseButtonDown(buttonInt)) StartCoroutine(ValidatedTask());
            }
            else
            {
                if (Input.GetMouseButtonUp(buttonInt)) StartCoroutine(ValidatedTask());
            }
        }
        else
        {
            if (Input.GetKeyDown(key)) StartCoroutine(ValidatedTask());
        }
    }

    public void ValidateTask()
    {
        StartCoroutine(ValidatedTask());
    }

    IEnumerator ValidatedTask()
    {
        isValidated = true;
        AudioManager.Instance.PlaySFX("Task_Success");
        var timeEllapsed = 0f;
        while (timeEllapsed < 0.3f)
        {
            background.color = new Color(1f - timeEllapsed*2f, 1f, 1f - timeEllapsed*2f, 1f);
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        // timeEllapsed = 0f;
        // while (timeEllapsed < 1f)
        // {
        //     rt.anchorMin += 3f * Time.deltaTime * Vector2.right;
        //     rt.anchorMax += 3f * Time.deltaTime * Vector2.right;
        //     timeEllapsed += Time.deltaTime;
        //     yield return null;
        // }
        timeEllapsed = 0;
        while (timeEllapsed < 1f)
        {
            cg.alpha = 1 - timeEllapsed;
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }  
        Destroy(gameObject);
    }

    public void ShowNewTask()
    {
        cor = StartCoroutine(appear());
    }

    IEnumerator appear()
    {
        var timeEllapsed = 0f;
        while (timeEllapsed < 0.9f)
        {
            cg.alpha = timeEllapsed;
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        cg.alpha = 0.9f;
        isActive = true;
    }
}
