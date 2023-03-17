using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGarbage : MonoBehaviour
{
    [SerializeField] DialogData intro2;
    private bool one;
    private bool two;
    private bool three;
    private bool four;
    private bool five;
    private bool six;
    private int baseChildCount;
    // Start is called before the first frame update
    void Start()
    {
       baseChildCount = transform.childCount;
       StartCoroutine(zeroCor());
    }

    // Update is called once per frame
    void Update()
    {
        if (!one && transform.childCount <= baseChildCount - 4) StartCoroutine(oneCor());
        if (!two && GameManager.Instance.heightPos == 1) {transform.GetChild(0).GetComponent<InfoBar>().ValidateTask(); two = true;}
        if (!three && GameManager.Instance.heightPos == 2) {Destroy(transform.GetChild(0).gameObject); three = true;}
        if (!four && GameManager.Instance.heightPos == 3) {transform.GetChild(0).GetComponent<InfoBar>().ValidateTask(); four = true;}
        if (!five && GameManager.Instance.heightPos == 4) {transform.GetChild(0).GetComponent<InfoBar>().ValidateTask(); five = true;}
        if (!six && GameManager.Instance.heightPos == 5) {transform.GetChild(0).GetComponent<InfoBar>().ValidateTask(); six = true;}
    }

    IEnumerator zeroCor()
    {
        yield return new WaitForSeconds(3f);
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        Destroy(transform.GetChild(0).gameObject);
    }

    IEnumerator oneCor()
    {
        DialogManager.Instance.Dialog(intro2);
        one = true;
        CinematicManager.cinematicPause = true;
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        CinematicManager.cinematicPause = false;
        Destroy(transform.GetChild(0).gameObject);
    }
}
