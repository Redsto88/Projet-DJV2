using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFocusBar : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider _focus;
    private PlayerManager _player;

    void Start()
    {
        _focus = GetComponent<Slider>();
        _player = PlayerManager.Instance;
    }

    void Update()
    {
        if (!_player.IsUnityNull())
        {
            _focus.value = _player.focus / _player.maxFocus;
        }
        else
        {
            _player = PlayerManager.Instance;
        }
    }
}
