using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject openClose;
    [SerializeField] private Corner corner;
    private bool isNear;
    public bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        openClose.SetActive(isOpen);
        if (isNear && isOpen)
        {
            if (Input.GetKeyDown(KeyCode.Space)) //TODO Show interaction button
            {
                StartCoroutine(RoomBehaviour.Instance.useDoor(corner));
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerController>() == PlayerController.Instance) //TODO optim ?
        {
            isNear = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<PlayerController>() == PlayerController.Instance) //TODO optim ?
        {
            isNear = false;
        }
    }
}
