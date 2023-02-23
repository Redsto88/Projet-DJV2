using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    [SerializeField] private float alphaTime;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject baseImage;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private DialogData test;
    private List<RectTransform> characters;
    private CanvasGroup cg;
    private bool skipDialog;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        cg = canvas.GetComponent<CanvasGroup>();
        characters = new List<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) skipDialog = true;
        if (Input.GetKeyDown(KeyCode.G)) Dialog(test);
    }

    public void Dialog(DialogData dd)
    {
        cg.alpha = 0;
        canvas.SetActive(true);
        StartCoroutine(changeAlpha(false));
        StartCoroutine(dialogCor(dd));
    }

    IEnumerator dialogCor(DialogData dd)
    {
        foreach (InitCharacters dc in dd.characters)
        {
            var img = Instantiate(baseImage);
            img.transform.SetParent(canvas.transform);
            img.transform.localRotation = Quaternion.identity;
            img.transform.localScale = Vector3.one;
            img.GetComponent<RectTransform>().anchorMin = new Vector2(dc.basePosition, 0.5f);
            img.GetComponent<RectTransform>().anchorMax = new Vector2(dc.basePosition, 0.5f);
            img.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            img.GetComponent<Image>().sprite = dc.character.SpriteByEmotion(dc.baseEmotion);
            if (dc.basePosition > 0.5f)
            {
                img.GetComponent<RectTransform>().eulerAngles = new Vector3(0,180,0);
            }
            characters.Add(img.GetComponent<RectTransform>());
        }
        foreach(DialogEvent ev in dd.events)
        {
            Debug.Log("here");
            switch (ev.type)
            {
                case DialogEventType.Fade :
                    var timeEllapsed = 0f;
                    var basePos = characters[ev.characterIndex].anchorMin;
                    var rend = characters[ev.characterIndex].GetComponent<Image>();
                    while (timeEllapsed < ev.transitionTime)
                    {
                        if (basePos.x <= 0.5f)
                        {
                            characters[ev.characterIndex].anchorMin = Vector2.Lerp(basePos, basePos - new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                            characters[ev.characterIndex].anchorMax = Vector2.Lerp(basePos, basePos - new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                        }
                        else 
                        {
                            characters[ev.characterIndex].anchorMin = Vector2.Lerp(basePos, basePos + new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                            characters[ev.characterIndex].anchorMax = Vector2.Lerp(basePos, basePos + new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                        }
                        rend.color = new Color(1,1,1,1 - timeEllapsed/ev.transitionTime);
                        timeEllapsed += Time.deltaTime;
                        yield return null;
                    }
                break;
                case DialogEventType.Appear :
                    var timeEllapsedAppear = 0f;
                    var basePosAppear = characters[ev.characterIndex].anchorMin;
                    var rendAppear = characters[ev.characterIndex].GetComponent<Image>();
                    while (timeEllapsedAppear < ev.transitionTime)
                    {
                        if (basePosAppear.x <= 0.5f)
                        {
                            characters[ev.characterIndex].anchorMin = Vector2.Lerp(basePosAppear, basePosAppear + new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                            characters[ev.characterIndex].anchorMax = Vector2.Lerp(basePosAppear, basePosAppear + new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                        }
                        else 
                        {
                            characters[ev.characterIndex].anchorMin = Vector2.Lerp(basePosAppear, basePosAppear - new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                            characters[ev.characterIndex].anchorMax = Vector2.Lerp(basePosAppear, basePosAppear - new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                        }
                        rendAppear.color = new Color(1,1,1,timeEllapsedAppear/ev.transitionTime);
                        timeEllapsedAppear += Time.deltaTime;
                        yield return null;
                    }
                break;
                case DialogEventType.Move :
                    var cpos = characters[ev.characterIndex].anchorMin.x;
                    switch (ev.transitionType)
                    {
                        case TransitionType.None : 
                            characters[ev.characterIndex].anchorMin = new Vector2(ev.position, 0.5f);
                            characters[ev.characterIndex].anchorMax = new Vector2(ev.position, 0.5f);
                        break;
                        case TransitionType.Linear :
                            var timeEllapsedMoveLinear = 0f;
                            while (timeEllapsedMoveLinear < ev.transitionTime)
                            {
                                characters[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(cpos, ev.position, timeEllapsedMoveLinear/ev.transitionTime),0.5f);
                                characters[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(cpos, ev.position, timeEllapsedMoveLinear/ev.transitionTime),0.5f);
                                timeEllapsedMoveLinear += Time.deltaTime;
                                yield return null;
                            }
                        break;
                        case TransitionType.Hyperbolic :
                            var timeEllapsedMoveHyperbolic = 0f;
                            while (timeEllapsedMoveHyperbolic < ev.transitionTime)
                            {
                                characters[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(characters[ev.characterIndex].anchorMin.x, ev.position, 0.03f),0.5f);
                                characters[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(characters[ev.characterIndex].anchorMax.x, ev.position, 0.03f),0.5f);
                                timeEllapsedMoveHyperbolic += Time.deltaTime;
                                yield return null;
                            }
                        break;
                    }
                break;
                case DialogEventType.Swap :
                    var c1pos = characters[ev.characterIndex].anchorMin.x;
                    var c2pos = characters[ev.otherCharacterIndex].anchorMin.x;
                    switch (ev.transitionType)
                    {
                        case TransitionType.None : 
                            characters[ev.characterIndex].anchorMin = new Vector2(c2pos, 0.5f);
                            characters[ev.characterIndex].anchorMax = new Vector2(c2pos, 0.5f);
                            characters[ev.otherCharacterIndex].anchorMin = new Vector2(c1pos, 0.5f);
                            characters[ev.otherCharacterIndex].anchorMax = new Vector2(c1pos, 0.5f);
                        break;
                        case TransitionType.Linear :
                            var timeEllapsedSwapLinear = 0f;
                            while (timeEllapsedSwapLinear < ev.transitionTime)
                            {
                                characters[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(c1pos, c2pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                                characters[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(c1pos, c2pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                                characters[ev.otherCharacterIndex].anchorMin = new Vector2(Mathf.Lerp(c2pos, c1pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                                characters[ev.otherCharacterIndex].anchorMax = new Vector2(Mathf.Lerp(c2pos, c1pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                                timeEllapsedSwapLinear += Time.deltaTime;
                                yield return null;
                            }
                        break;
                        case TransitionType.Hyperbolic :
                            var timeEllapsedSwapHyperbolic = 0f;
                            while (timeEllapsedSwapHyperbolic < ev.transitionTime)
                            {
                                characters[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(characters[ev.characterIndex].anchorMin.x, c2pos, 0.03f),0.5f);
                                characters[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(characters[ev.characterIndex].anchorMax.x, c2pos, 0.03f),0.5f);
                                characters[ev.otherCharacterIndex].anchorMin = new Vector2(Mathf.Lerp(characters[ev.otherCharacterIndex].anchorMin.x, c1pos, 0.03f),0.5f);
                                characters[ev.otherCharacterIndex].anchorMax = new Vector2(Mathf.Lerp(characters[ev.otherCharacterIndex].anchorMax.x, c1pos, 0.03f),0.5f);
                                timeEllapsedSwapHyperbolic += Time.deltaTime;
                                yield return null;
                            }
                        break;
                    }
                break;
                case DialogEventType.Talk :
                    txt.text = "";
                    characters[ev.characterIndex].GetComponent<Image>().sprite = dd.characters[ev.characterIndex].character.SpriteByEmotion(ev.emotion); //TODO add anim possibilities
                    int k = 0;
                    while (k < ev.text.Length && !skipDialog) //TODO Change with custom InputManager
                    {
                        txt.text += ev.text[k];
                        if (ev.makesSound)
                        {
                            //Change the tone with a random value between ev.toneVariation.x and ev.toneVariation.y
                            //Play the sound with ev.sound
                        }
                        k++;
                        yield return new WaitForSeconds(ev.textSpeed/1000f);
                    }
                    skipDialog = false;
                    txt.text = ev.text;
                    yield return null;
                    while (!skipDialog) yield return null;
                    skipDialog = false;
                    yield return null;
                break;
            }
        }
        foreach (RectTransform rt in characters)
        {
            Destroy(rt.gameObject);
        }
        characters.Clear();
        yield return null;
    }

    IEnumerator changeAlpha(bool fade)
    {
        var timeEllapsed = 0f;
        while (timeEllapsed < alphaTime)
        {
            cg.alpha = (fade) ? 1 - timeEllapsed/alphaTime : timeEllapsed/alphaTime;
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = (fade) ? 0 : 1;
    }

}
