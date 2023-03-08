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
    
    [SerializeField] private TextMeshProUGUI charName;
    [SerializeField] private DialogData test;
    private List<RectTransform> characterHolders;
    private List<DialogCharacter> characters;
    private CanvasGroup cg;
    private bool skipDialog;
    private bool isInThought;
    private bool canSkipDialog;
    private Image textBox;
    private List<bool> isEventFinished;
    private Coroutine dialogue;
    public bool inDialog => dialogue != null;
    private int currentEventID = 0;

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
        characterHolders = new List<RectTransform>();
        characters = new List<DialogCharacter>();
        textBox = canvas.transform.GetChild(3).GetComponent<Image>();
        isEventFinished = new List<bool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSkipDialog) skipDialog = true;
        if (Input.GetKeyDown(KeyCode.G)) Dialog(test);
    }

    public void Dialog(DialogData dd)
    {
        cg.alpha = 0;
        txt.text = "";
        charName.text = "";
        canvas.SetActive(true);
        StartCoroutine(changeAlpha(false));
        dialogue = StartCoroutine(dialogCor(dd));
    }

    IEnumerator dialogCor(DialogData dd)
    {
        textBox.color = new Color(1,1,1,dd.startInThought ? 0 : 1);
        isInThought = dd.startInThought;
        isEventFinished.Clear();
        foreach (InitCharacters dc in dd.characters)
        {
            var img = Instantiate(baseImage);
            img.transform.SetParent(canvas.transform);
            img.transform.SetSiblingIndex(0);
            img.transform.localRotation = Quaternion.identity;
            img.transform.localScale = Vector3.one;
            img.GetComponent<RectTransform>().anchorMin = new Vector2(dc.basePosition, 0.5f);
            img.GetComponent<RectTransform>().anchorMax = new Vector2(dc.basePosition, 0.5f);
            img.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            img.GetComponent<Image>().sprite = dc.character.SpriteByEmotion(dc.baseEmotion);
            img.GetComponent<Image>().color = new Color(1,1,1,dc.isVisible ? 1 : 0);
            img.GetComponent<Image>().SetNativeSize();
            if (dc.basePosition > 0.5f)
            {
                img.GetComponent<RectTransform>().localEulerAngles = new Vector3(0,180,0);
            }
            img.transform.GetChild(0).GetComponent<Image>().color = new Color(0,0,0,dc.IsInLight ? 0 : 0.75f);
            characterHolders.Add(img.GetComponent<RectTransform>());
            characters.Add(dc.character);
        }
        for (int i = 0; i < dd.events.Count; i++)
        {
            isEventFinished.Add(false);
        }
        currentEventID = 0;
        foreach(DialogEvent ev in dd.events)
        {
            switch (ev.type)
            {
                case DialogEventType.Wait : StartCoroutine(WaitEvent(ev,currentEventID)); break;
                case DialogEventType.Fade : StartCoroutine(FadeEvent(ev,currentEventID)); break;
                case DialogEventType.Appear : StartCoroutine(AppearEvent(ev,currentEventID)); break;
                case DialogEventType.InLight : StartCoroutine(InLightEvent(ev,currentEventID)); break;
                case DialogEventType.OutLight : StartCoroutine(OutLightEvent(ev,currentEventID)); break;
                case DialogEventType.Move : StartCoroutine(MoveEvent(ev,currentEventID)); break;
                case DialogEventType.Swap : StartCoroutine(SwapEvent(ev,currentEventID)); break;
                case DialogEventType.Talk : StartCoroutine(TalkEvent(dd,ev,currentEventID)); break;
            }
            currentEventID++;
            var k = currentEventID;
            if (!ev.parallel) yield return new WaitUntil(() => previousFinished(k));
            Debug.Log(currentEventID);
        }
        foreach (RectTransform rt in characterHolders)
        {
            Destroy(rt.gameObject);
        }
        characterHolders.Clear();
        characters.Clear();
        yield return null;
        StartCoroutine(changeAlpha(true));
        dialogue = null;
    }

    bool previousFinished(int k)
    {
        for (int i = 0; i < k; i++)
        {
            if (!isEventFinished[i]) return false;
        }
        return true;
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
        if (fade) canvas.SetActive(false);
    }

    IEnumerator WaitEvent(DialogEvent ev, int k)
    {
        yield return new WaitForSeconds(ev.transitionTime);
        isEventFinished[k] = true;
    }

    IEnumerator FadeEvent(DialogEvent ev, int k)
    {
        var timeEllapsed = 0f;
        var basePos = characterHolders[ev.characterIndex].anchorMin;
        var rend = characterHolders[ev.characterIndex].GetComponent<Image>();
        while (timeEllapsed < ev.transitionTime)
        {
            if (basePos.x <= 0.5f)
            {
                characterHolders[ev.characterIndex].anchorMin = Vector2.Lerp(basePos, basePos - new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                characterHolders[ev.characterIndex].anchorMax = Vector2.Lerp(basePos, basePos - new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
            }
            else 
            {
                characterHolders[ev.characterIndex].anchorMin = Vector2.Lerp(basePos, basePos + new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
                characterHolders[ev.characterIndex].anchorMax = Vector2.Lerp(basePos, basePos + new Vector2(0.2f,0), timeEllapsed/ev.transitionTime);
            }
            rend.color = new Color(1,1,1,1 - timeEllapsed/ev.transitionTime);
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        isEventFinished[k] = true;
    }

    IEnumerator AppearEvent(DialogEvent ev, int k)
    {
        var timeEllapsedAppear = 0f;
        var basePosAppear = characterHolders[ev.characterIndex].anchorMin;
        var rendAppear = characterHolders[ev.characterIndex].GetComponent<Image>();
        while (timeEllapsedAppear < ev.transitionTime)
        {
            if (basePosAppear.x <= 0.5f)
            {
                characterHolders[ev.characterIndex].anchorMin = Vector2.Lerp(basePosAppear, basePosAppear + new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                characterHolders[ev.characterIndex].anchorMax = Vector2.Lerp(basePosAppear, basePosAppear + new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
            }
            else 
            {
                characterHolders[ev.characterIndex].anchorMin = Vector2.Lerp(basePosAppear, basePosAppear - new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
                characterHolders[ev.characterIndex].anchorMax = Vector2.Lerp(basePosAppear, basePosAppear - new Vector2(0.2f,0), timeEllapsedAppear/ev.transitionTime);
            }
            rendAppear.color = new Color(1,1,1,timeEllapsedAppear/ev.transitionTime);
            timeEllapsedAppear += Time.deltaTime;
            yield return null;
        }
        isEventFinished[k] = true;
    }

    IEnumerator InLightEvent(DialogEvent ev, int k)
    {
        var timeEllapsedInLight = 0f;
        var rendInLight = characterHolders[ev.characterIndex].transform.GetChild(0).GetComponent<Image>();
        while (timeEllapsedInLight < ev.transitionTime)
        {
            rendInLight.color = new Color(0,0,0,Mathf.Lerp(0.75f,0f,timeEllapsedInLight/ev.transitionTime));
            timeEllapsedInLight += Time.deltaTime;
            yield return null;
        }
        isEventFinished[k] = true;
    }

    IEnumerator OutLightEvent(DialogEvent ev, int k)
    {
        var timeEllapsedOutLight = 0f;
        var rendOutLight = characterHolders[ev.characterIndex].transform.GetChild(0).GetComponent<Image>();
        while (timeEllapsedOutLight < ev.transitionTime)
        {
            rendOutLight.color = new Color(0,0,0,Mathf.Lerp(0f,0.75f,timeEllapsedOutLight/ev.transitionTime));
            timeEllapsedOutLight += Time.deltaTime;
            yield return null;
        }
        isEventFinished[k] = true;
    }

    IEnumerator MoveEvent(DialogEvent ev, int k)
    {
        var cpos = characterHolders[ev.characterIndex].anchorMin.x;
        characterHolders[ev.characterIndex].localEulerAngles = new Vector3(0,(ev.position <= 0.5f ? 0 : 180),0);
        switch (ev.transitionType)
        {
            case TransitionType.None : 
                characterHolders[ev.characterIndex].anchorMin = new Vector2(ev.position, 0.5f);
                characterHolders[ev.characterIndex].anchorMax = new Vector2(ev.position, 0.5f);
            break;
            case TransitionType.Linear :
                var timeEllapsedMoveLinear = 0f;
                while (timeEllapsedMoveLinear < ev.transitionTime)
                {
                    characterHolders[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(cpos, ev.position, timeEllapsedMoveLinear/ev.transitionTime),0.5f);
                    characterHolders[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(cpos, ev.position, timeEllapsedMoveLinear/ev.transitionTime),0.5f);
                    timeEllapsedMoveLinear += Time.deltaTime;
                    yield return null;
                }
            break;
            case TransitionType.Hyperbolic :
                var timeEllapsedMoveHyperbolic = 0f;
                while (timeEllapsedMoveHyperbolic < ev.transitionTime)
                {
                    characterHolders[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(characterHolders[ev.characterIndex].anchorMin.x, ev.position, 0.03f),0.5f);
                    characterHolders[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(characterHolders[ev.characterIndex].anchorMax.x, ev.position, 0.03f),0.5f);
                    timeEllapsedMoveHyperbolic += Time.deltaTime;
                    yield return null;
                }
            break;
        }
        isEventFinished[k] = true;
    }

    IEnumerator SwapEvent(DialogEvent ev, int k)
    {
        var c1pos = characterHolders[ev.characterIndex].anchorMin.x;
        var c2pos = characterHolders[ev.otherCharacterIndex].anchorMin.x;
        characterHolders[ev.characterIndex].localEulerAngles = new Vector3(0,(c2pos <= 0.5f ? 0 : 180),0);
        characterHolders[ev.otherCharacterIndex].localEulerAngles = new Vector3(0,(c1pos <= 0.5f ? 0 : 180),0);
        switch (ev.transitionType)
        {
            case TransitionType.None : 
                characterHolders[ev.characterIndex].anchorMin = new Vector2(c2pos, 0.5f);
                characterHolders[ev.characterIndex].anchorMax = new Vector2(c2pos, 0.5f);
                characterHolders[ev.otherCharacterIndex].anchorMin = new Vector2(c1pos, 0.5f);
                characterHolders[ev.otherCharacterIndex].anchorMax = new Vector2(c1pos, 0.5f);
            break;
            case TransitionType.Linear :
                var timeEllapsedSwapLinear = 0f;
                while (timeEllapsedSwapLinear < ev.transitionTime)
                {
                    characterHolders[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(c1pos, c2pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                    characterHolders[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(c1pos, c2pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                    characterHolders[ev.otherCharacterIndex].anchorMin = new Vector2(Mathf.Lerp(c2pos, c1pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                    characterHolders[ev.otherCharacterIndex].anchorMax = new Vector2(Mathf.Lerp(c2pos, c1pos, timeEllapsedSwapLinear/ev.transitionTime),0.5f);
                    timeEllapsedSwapLinear += Time.deltaTime;
                    yield return null;
                }
            break;
            case TransitionType.Hyperbolic :
                var timeEllapsedSwapHyperbolic = 0f;
                while (timeEllapsedSwapHyperbolic < ev.transitionTime)
                {
                    characterHolders[ev.characterIndex].anchorMin = new Vector2(Mathf.Lerp(characterHolders[ev.characterIndex].anchorMin.x, c2pos, 0.03f),0.5f);
                    characterHolders[ev.characterIndex].anchorMax = new Vector2(Mathf.Lerp(characterHolders[ev.characterIndex].anchorMax.x, c2pos, 0.03f),0.5f);
                    characterHolders[ev.otherCharacterIndex].anchorMin = new Vector2(Mathf.Lerp(characterHolders[ev.otherCharacterIndex].anchorMin.x, c1pos, 0.03f),0.5f);
                    characterHolders[ev.otherCharacterIndex].anchorMax = new Vector2(Mathf.Lerp(characterHolders[ev.otherCharacterIndex].anchorMax.x, c1pos, 0.03f),0.5f);
                    timeEllapsedSwapHyperbolic += Time.deltaTime;
                    yield return null;
                }
            break;
        }
        isEventFinished[k] = true;
    }

    IEnumerator TalkEvent(DialogData dd, DialogEvent ev, int i)
    {
        if (ev.isThought != isInThought)
        {
            var thoughtTransition = 0f;
            while (thoughtTransition < 0.5f)
            {
                textBox.color = new Color(1,1,1,ev.isThought ? Mathf.Lerp(1f,0f,thoughtTransition*2f) : Mathf.Lerp(0f,1f, thoughtTransition*2f));
                thoughtTransition += Time.deltaTime;
                yield return null;
            }
            isInThought = ev.isThought;
        }
        txt.text = "";
        charName.text = characters[ev.characterIndex].characterName;
        characterHolders[ev.characterIndex].GetComponent<Image>().sprite = dd.characters[ev.characterIndex].character.SpriteByEmotion(ev.emotion); //TODO add anim possibilities
        int k = 0;
        canSkipDialog = true;
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
        canSkipDialog = false;
        yield return null;
        isEventFinished[i] = true;
    }

}
