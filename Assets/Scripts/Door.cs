using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum Corner
    {
        Up,
        Left,
        Right,
        Down
    }
    [SerializeField] private Corner corner;
    [SerializeField] private Animator bariereAnimator;
    private bool _isNear;
    private bool _doorAnim = false;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //openClose.SetActive(isOpen);
        if (isOpen && !_doorAnim)
        {
            _doorAnim = true;
            bariereAnimator.CrossFade("Open", 0.01f);
        }
        if (_isNear && isOpen)
        {
            if (Input.GetButton("Interaction")) //TODO Show interaction button
            {
                AudioManager.Instance.PlaySFX("RoomTransition");
                StartCoroutine(RoomBehaviour.Instance.useDoor(corner));
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerController>() == PlayerController.Instance) //TODO optim ?
        {
            _isNear = true;
            PlayerController.Instance.GetComponentInChildren<UITexts>().ToggleInteractionText();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerController>() == PlayerController.Instance) //TODO optim ?
        {
            _isNear = false;
            PlayerController.Instance.GetComponentInChildren<UITexts>().ToggleInteractionText();
        }
    }
}
