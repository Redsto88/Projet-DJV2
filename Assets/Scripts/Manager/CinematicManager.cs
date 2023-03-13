using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class CinematicManager : MonoBehaviour
{
    public int startingCinematic;
    public static CinematicManager Instance;
    public static bool cinematicPause;
    public static List<int> seen = new List<int>();
    [SerializeField] private Image filter;
    private GameObject canvas;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        transform.SetParent(null);
        transform.SetSiblingIndex(1);
        canvas = filter.transform.parent.gameObject;
    }

    void Start()
    {
        if (seen.Contains(startingCinematic)) return;
        switch (startingCinematic)
        {
            case 0: break;
            case 1: StartCoroutine(PortalTuto()); break;
            case 2: StartCoroutine(FirstAttackTuto()); break;
            case 3: StartCoroutine(PuzzleTuto()); break;
            case 4: StartCoroutine(DocksTuto()); break;
        }
    }

    public void StartCinematic(int i)
    {
        if (seen.Contains(i)) return;
        switch (i)
        {
            case 0: break;
            case 1: StartCoroutine(PortalTuto()); break;
            case 2: StartCoroutine(FirstAttackTuto()); break;
            case 3: StartCoroutine(PuzzleTuto()); break;
            case 4: StartCoroutine(DocksTuto()); break;
        }
    }
    
    public IEnumerator PortalTuto()
    {
        cinematicPause = true;
        canvas.SetActive(true);
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
        canvas.SetActive(false);
        seen.Add(1);
        cinematicPause = false;
        // DialogData dd2 = Addressables.LoadAssetAsync<DialogData>("IntroductionDialog2").WaitForCompletion();
        // DialogManager.Instance.Dialog(dd2);
        // yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        // seen.Add(1);
        // cinematicPause = false;
    }

    public IEnumerator FirstAttackTuto()
    {
        cinematicPause = true;
        MainCamera.Instance.followTarget.targetPoint = new Vector3(9,0,0) + RoomBehaviour.Instance.transform.position;
        MainCamera.Instance.followTarget.aimPoint = true;
        yield return new WaitForSeconds(1);
        DialogData dd = Addressables.LoadAssetAsync<DialogData>("FirstAttackDialog").WaitForCompletion();
        DialogManager.Instance.Dialog(dd);
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        MainCamera.Instance.followTarget.aimPoint = false;
        RoomBehaviour.Instance.ActivateEnnemies();
        seen.Add(2);
        cinematicPause = false;
    }

    public IEnumerator PuzzleTuto()
    {
        cinematicPause = true;
        DialogData dd = Addressables.LoadAssetAsync<DialogData>("PuzzleTutoDialog").WaitForCompletion();
        DialogManager.Instance.Dialog(dd);
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        seen.Add(3);
        cinematicPause = false;
    }

    public IEnumerator DocksTuto()
    {
        cinematicPause = true;
        MainCamera.Instance.followTarget.targetPoint = new Vector3(11.4f,3f,10f) + RoomBehaviour.Instance.transform.position;
        MainCamera.Instance.followTarget.aimPoint = true;
        var timeEllapsed = 0f;
        yield return new WaitForSeconds(0.5f);
        while (timeEllapsed < 4f)
        {
            MainCamera.Instance.followTarget.targetPoint -= 4 * Time.deltaTime * Vector3.forward;
            timeEllapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        MainCamera.Instance.followTarget.aimPoint = false;
        DialogData dd = Addressables.LoadAssetAsync<DialogData>("DocksTutoDialog").WaitForCompletion();
        DialogManager.Instance.Dialog(dd);
        yield return new WaitWhile(() => DialogManager.Instance.inDialog);
        RoomBehaviour.Instance.ActivateEnnemies();
        seen.Add(4);
        cinematicPause = false;
    }
}
