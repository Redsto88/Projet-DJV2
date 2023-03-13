using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private MapTile mapTilePrefab;
    [SerializeField] private Transform content;
    [SerializeField] private RectTransform leftPanel;
    [SerializeField] private RectTransform rightPanel;
    private MapTile[,] mapTiles;

    [SerializeField] private GameObject map;
    public bool isInit = false;
    public bool paused = false;
    private Coroutine movement;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Init()
    {
        if (isInit) return;
        isInit = true;
        mapTiles = new MapTile[GameManager.Instance.dungeonHeight,GameManager.Instance.dungeonWidth];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !CinematicManager.cinematicPause)
        {
            content.GetComponent<RectTransform>().anchoredPosition = - 0.007f * new Vector2(GameManager.Instance.heightPos * content.GetComponent<RectTransform>().rect.height, GameManager.Instance.widthPos * content.GetComponent<RectTransform>().rect.width);
            EnterOrExitMenu();
        }
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (axis.magnitude > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            SetSelected();
        }
    }

    void SetSelected(){
        List<Button> buttonList = new List<Button>(GetComponentsInChildren<Button>());
        //get the first active button
        Button firstButton = buttonList.Find(x => x.gameObject.activeInHierarchy);
        if(firstButton != null) EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        
    }

    public void PlaceTile(RoomData roomData, int h, int w)
    {
        var tile = Instantiate(mapTilePrefab);
        tile.transform.SetParent(content);
        tile.GetComponent<RectTransform>().anchorMin = new Vector2(0.4975f,0.4975f) + 0.007f * new Vector2(h, w);
        tile.GetComponent<RectTransform>().anchorMax = new Vector2(0.5025f,0.5025f) + 0.007f * new Vector2(h, w);
        tile.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        tile.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        tile.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        tile.transform.localRotation = Quaternion.identity;
        tile.transform.localScale = Vector3.one;
        tile.GetComponent<MapTile>().upDoorIcn.SetActive(roomData.hasUpDoor);
        tile.GetComponent<MapTile>().leftDoorIcn.SetActive(roomData.hasLeftDoor);
        tile.GetComponent<MapTile>().rightDoorIcn.SetActive(roomData.hasRightDoor);
        tile.GetComponent<MapTile>().downDoorIcn.SetActive(roomData.hasDownDoor);
        tile.icon.sprite = sprites[(int)roomData.mapType];
        if (tile.icon.sprite == null) tile.icon.color = new Color(0,0,0,0);
        mapTiles[h,w] = tile;
        tile.gameObject.SetActive(false);
    }

    public void TileSeen(int h, int w)
    {
        mapTiles[h,w].gameObject.SetActive(true);
    }

    public void TileCleared(int h, int w)
    {
        mapTiles[h,w].grayed.SetActive(false);
    }

    public void LeavesTile(int h, int w)
    {
        mapTiles[h,w].notPresentMask.SetActive(true);
        mapTiles[h,w].grayed.SetActive((int)GameManager.Instance.roomState[h,w] < 2);
    }

    public void GoesOnTile(int h, int w)
    {
        mapTiles[h,w].notPresentMask.SetActive(false);
        mapTiles[h,w].grayed.SetActive(false);
        if (GameManager.Instance.GetCurrentRoomData().hasUpDoor && h+1 < GameManager.Instance.dungeonHeight && GameManager.Instance.GetRoomData(h+1,w) != null && GameManager.Instance.GetRoomData(h+1,w).hasDownDoor) TileSeen(h+1,w);
        if (GameManager.Instance.GetCurrentRoomData().hasLeftDoor && w+1 < GameManager.Instance.dungeonWidth && GameManager.Instance.GetRoomData(h,w+1) != null && GameManager.Instance.GetRoomData(h,w+1).hasRightDoor) TileSeen(h,w+1);
        if (GameManager.Instance.GetCurrentRoomData().hasDownDoor && h > 0 && GameManager.Instance.GetRoomData(h-1,w) != null && GameManager.Instance.GetRoomData(h-1,w).hasUpDoor) TileSeen(h-1,w);
        if (GameManager.Instance.GetCurrentRoomData().hasRightDoor && w > 0 && GameManager.Instance.GetRoomData(h,w-1) != null && GameManager.Instance.GetRoomData(h,w-1).hasLeftDoor) TileSeen(h,w-1);
    }

    public void EnterOrExitMenu()
    {
        if (movement != null) return;
        paused = !paused;
        if (paused) {TimeManager.Instance.Pause(); movement = StartCoroutine(showMenu());}
        else {TimeManager.Instance.Unpause(); movement = StartCoroutine(hideMenu());}
        map.SetActive(paused);
    }

    public void GoToMainMenu()
    {
        EnterOrExitMenu();
        SceneManager.LoadScene("Menu");
    }

    public void ShowOptions()
    {

    }

    IEnumerator showMenu()
    {
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        var timeEllapsed = 0f;
        while (timeEllapsed < 1f/6f)
        {
            leftPanel.anchorMin = (timeEllapsed * 6 - 1) * Vector2.right;
            leftPanel.anchorMax = timeEllapsed * 6 * Vector2.right + Vector2.up;
            rightPanel.anchorMin = (-timeEllapsed * 6 + 1) * Vector2.right;
            rightPanel.anchorMax = (-timeEllapsed * 6 + 2) * Vector2.right + Vector2.up;
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        leftPanel.anchorMin = Vector2.zero;
        leftPanel.anchorMax = Vector2.one;
        rightPanel.anchorMin = Vector2.zero;
        rightPanel.anchorMax = Vector2.one;
        movement = null;
    }

    IEnumerator hideMenu()
    {
        //hide cursor
        Cursor.visible = false;
        var timeEllapsed = 0f;
        while (timeEllapsed < 1f/6f)
        {
            leftPanel.anchorMin = -timeEllapsed * 6 * Vector2.right;
            leftPanel.anchorMax = (1 - timeEllapsed * 6) * Vector2.right + Vector2.up;
            rightPanel.anchorMin = timeEllapsed * 6 * Vector2.right;
            rightPanel.anchorMax = (1 + timeEllapsed * 6) * Vector2.right + Vector2.up;
            timeEllapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        leftPanel.anchorMin = -Vector2.right;
        leftPanel.anchorMax = Vector2.up;
        rightPanel.anchorMin = Vector2.right;
        rightPanel.anchorMax = Vector2.one + Vector2.right;
        movement = null;
    }
}
