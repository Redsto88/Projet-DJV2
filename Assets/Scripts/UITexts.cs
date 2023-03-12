using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITexts : MonoBehaviour
{
    public TMP_Text textChangementSalle;
    
    // Start is called before the first frame update
    void Start()
    {
        textChangementSalle.enabled = false;
    }

    public void ToggleInteractionText(bool activate)
    {
        textChangementSalle.enabled = activate;
    }
}
