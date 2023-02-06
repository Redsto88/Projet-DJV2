using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private MapTile mapTilePrefab;
    [SerializeField] private Transform content;
    private MapTile[,] mapTiles;

    public bool isInit = false;

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
        if (Input.GetKeyDown(KeyCode.V))
        {
            content.GetComponent<RectTransform>().anchoredPosition = -15 * new Vector3(GameManager.Instance.heightPos, GameManager.Instance.widthPos, 0);
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
    }

    public void PlaceTile(RoomData roomData, int h, int w)
    {
        var tile = Instantiate(mapTilePrefab);
        tile.transform.SetParent(content);
        tile.GetComponent<RectTransform>().anchoredPosition = 15 * new Vector3(h,w,0);
        tile.transform.localRotation = Quaternion.identity;
        tile.GetComponent<RectTransform>().localScale = 0.1f * Vector3.one;
        tile.GetComponent<MapTile>().upDoorIcn.SetActive(roomData.hasUpDoor);
        tile.GetComponent<MapTile>().leftDoorIcn.SetActive(roomData.hasLeftDoor);
        tile.GetComponent<MapTile>().rightDoorIcn.SetActive(roomData.hasRightDoor);
        tile.GetComponent<MapTile>().downDoorIcn.SetActive(roomData.hasDownDoor);
        tile.icon.sprite = sprites[(int)roomData.mapType];
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
        if (GameManager.Instance.GetCurrentRoomData().hasUpDoor && h+1 < GameManager.Instance.dungeonHeight && GameManager.Instance.GetRoomData(h+1,w).hasDownDoor) TileSeen(h+1,w);
        if (GameManager.Instance.GetCurrentRoomData().hasLeftDoor && w+1 < GameManager.Instance.dungeonWidth && GameManager.Instance.GetRoomData(h,w+1).hasRightDoor) TileSeen(h,w+1);
        if (GameManager.Instance.GetCurrentRoomData().hasDownDoor && h > 0 && GameManager.Instance.GetRoomData(h-1,w).hasUpDoor) TileSeen(h-1,w);
        if (GameManager.Instance.GetCurrentRoomData().hasRightDoor && w > 0 && GameManager.Instance.GetRoomData(h,w-1).hasLeftDoor) TileSeen(h,w-1);
    }
}
