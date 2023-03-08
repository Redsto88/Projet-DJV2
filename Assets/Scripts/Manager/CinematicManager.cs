using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class CinematicManager : MonoBehaviour
{
    public int startingCinematic;
    public static CinematicManager Instance;
    [SerializeField] private Transform camTarget;
    [SerializeField] private Image filter;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        transform.SetParent(null);
        transform.SetSiblingIndex(1);
    }

    void Start()
    {
        switch (startingCinematic)
        {
            case 0: break;
            case 1: StartCoroutine(PortalTuto()); break;
        }
    }
    
    public IEnumerator PortalTuto()
    {
        camTarget.gameObject.SetActive(false);
        filter.color = Color.black;
        DialogData dd = Addressables.LoadAssetAsync<DialogData>("IntroductionDialog1").WaitForCompletion();
        DialogManager.Instance.Dialog(dd);
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        var timeEllapsed = 0f;
        while (timeEllapsed < 2)
        {
            filter.color = new Color(0,0,0,1 - timeEllapsed/2f);
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        DialogData dd2 = Addressables.LoadAssetAsync<DialogData>("IntroductionDialog2").WaitForCompletion();
        DialogManager.Instance.Dialog(dd2);
    }
}
