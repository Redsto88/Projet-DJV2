using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TMPFontAutoSizeMultiLine : MonoBehaviour
{
    [SerializeField] private float lineNumber;
    private RectTransform rt;
    private TextMeshProUGUI txt;
    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        txt = GetComponent<TextMeshProUGUI>();  
    }

    // Update is called once per frame
    void Update()
    {
        txt.fontSize = 3f * rt.rect.height / (lineNumber + txt.lineSpacing * (lineNumber-1)) / 4f;
    }
}
