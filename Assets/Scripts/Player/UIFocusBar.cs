using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFocusBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Slider _focus;
    private PlayerManager player;

    void Start()
    {
        player = PlayerManager.Instance;
    }

    void Update()
    {
        if (player != null)
        _focus.value = player.focus / player.maxFocus;
        else player = PlayerManager.Instance;
    }
}
