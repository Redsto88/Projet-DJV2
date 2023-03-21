using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int dungeonWidth = 3;
    public int dungeonHeight = 3;
    public RoomData[,] dungeonData;
    public enum RoomState
    {
        NotSeen,
        Seen,
        Cleared
    }
    public RoomState[,] roomState;
    [System.Serializable]
    public class WeightedRoom
    {
        public RoomData roomData;
        public int weight = 1;
    };
    public List<WeightedRoom> possibleRooms;
    public int heightPos = 0;
    public int widthPos = 0;

    [Header("Death Manager")]
    [SerializeField] private GameObject deathScreen;

    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
            MapManager.Instance?.Init();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(playerPrefab, transform.position, Quaternion.Euler(0,45,0));
        MapManager.Instance.Init();
        dungeonData = new RoomData[dungeonHeight,dungeonWidth];
        roomState = new RoomState[dungeonHeight,dungeonWidth];
        GenerateDungeon();
        Instantiate(dungeonData[0,0].roomPrefab);
        seenRoom();
        MapManager.Instance.GoesOnTile(0,0);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     TPToRoom(0,0);
        // }
    }

    void GenerateDungeon()
    {
        // for (int i = 0; i < dungeonHeight; i++)
        // {
        //     for (int j = 0; j < dungeonWidth; j++)
        //     {
        //         dungeonData[i,j] = GetRandomRoom(); //TODO prendre en compte les portes ?
        //         roomState[i,j] = RoomState.NotSeen;
        //         MapManager.Instance.PlaceTile(dungeonData[i,j], i, j);
        //     }
        // }
        heightPos = 0;
        widthPos = 0;
        dungeonData[0,0] = possibleRooms[0].roomData;
        dungeonData[1,0] = possibleRooms[1].roomData;
        dungeonData[2,0] = possibleRooms[2].roomData;
        dungeonData[3,0] = possibleRooms[3].roomData;
        dungeonData[4,0] = possibleRooms[4].roomData;
        dungeonData[4,1] = possibleRooms[6].roomData;
        dungeonData[5,0] = possibleRooms[5].roomData;
        roomState[0,0] = RoomState.Seen; roomState[1,0] = RoomState.NotSeen;
        roomState[2,0] = RoomState.NotSeen; roomState[3,0] = RoomState.NotSeen;
        roomState[4,0] = RoomState.NotSeen; roomState[4,1] = RoomState.NotSeen; roomState[5,0] = RoomState.NotSeen;
        MapManager.Instance.PlaceTile(dungeonData[0,0], 0, 0);
        MapManager.Instance.PlaceTile(dungeonData[1,0], 1, 0);
        MapManager.Instance.PlaceTile(dungeonData[2,0], 2, 0);
        MapManager.Instance.PlaceTile(dungeonData[3,0], 3, 0);
        MapManager.Instance.PlaceTile(dungeonData[4,0], 4, 0);
        MapManager.Instance.PlaceTile(dungeonData[4,1], 4, 1);
        MapManager.Instance.PlaceTile(dungeonData[5,0], 5, 0);
    }

    RoomData GetRandomRoom()
    {
        int k = 0;
        int total = 0;
        foreach(WeightedRoom r in possibleRooms)
        {
            total += r.weight;
        }
        int index = Random.Range(0,total);
        foreach(WeightedRoom r in possibleRooms)
        {
            for (int j = 0; j < r.weight; j++)
            {
                if (k == index) return r.roomData;
                k++;
            }
        }
        return null;
    }

    public void GoToNextRoom(Door.Corner corner)
    {
        MapManager.Instance.LeavesTile(heightPos, widthPos);
        switch (corner)
        {
            case Door.Corner.Up: heightPos++; break;
            case Door.Corner.Left: widthPos++; break;
            case Door.Corner.Right: widthPos--; break;
            case Door.Corner.Down: heightPos--; break;
        }
        MapManager.Instance.GoesOnTile(heightPos, widthPos);
        Instantiate(dungeonData[heightPos,widthPos].roomPrefab, 50 * (Vector3.right * heightPos + Vector3.forward * widthPos), Quaternion.identity);
        RoomBehaviour.Instance.closeDoor(corner);
        PlayerController.Instance.GetComponent<SpawnPortal>().DeletePortals();
    }

    public void TPToRoom(int h, int w)
    {
        MapManager.Instance.LeavesTile(heightPos, widthPos);
        PlayerController.Instance.characterController.enabled = false;
        Instantiate(dungeonData[heightPos,widthPos].roomPrefab,50 * (Vector3.right * h + Vector3.forward * w), Quaternion.identity);
        PlayerController.Instance.transform.position = 50 * (Vector3.right * h + Vector3.forward * w);
        PlayerController.Instance.characterController.enabled = true;
        heightPos = h;
        widthPos = w;
        MapManager.Instance.GoesOnTile(heightPos, widthPos);
        PlayerController.Instance.GetComponent<SpawnPortal>().DeletePortals();
    }

    public bool HasNextRoom(Door.Corner corner)
    {
        switch (corner)
        {
            case Door.Corner.Up: if (heightPos+1 < dungeonHeight) return dungeonData[heightPos+1, widthPos] != null && dungeonData[heightPos+1, widthPos].hasDownDoor; break;
            case Door.Corner.Left: if (widthPos+1 < dungeonWidth) return dungeonData[heightPos, widthPos+1] != null && dungeonData[heightPos, widthPos+1].hasRightDoor; break;
            case Door.Corner.Right: if (widthPos > 0) return dungeonData[heightPos, widthPos-1] != null && dungeonData[heightPos, widthPos-1].hasLeftDoor; break;
            case Door.Corner.Down: if (heightPos > 0) return dungeonData[heightPos-1, widthPos] != null && dungeonData[heightPos-1, widthPos].hasUpDoor; break;
        }
        return false;
    }

    public RoomData GetCurrentRoomData()
    {
        return dungeonData[heightPos,widthPos];
    }

    public RoomData GetRoomData(int h, int w)
    {
        return dungeonData[h,w];
    }

    public bool isCurrentRoomClear()
    {
        return roomState[heightPos,widthPos] == RoomState.Cleared;
    }

    public void seenRoom()
    {
        roomState[heightPos,widthPos] = RoomState.Seen;
        MapManager.Instance.TileSeen(heightPos,widthPos);
    }

    public void clearedRoom()
    {
        roomState[heightPos,widthPos] = RoomState.Cleared;
        MapManager.Instance.TileCleared(heightPos,widthPos);
    }

    public void onPlayerDeath()
    {
        Cursor.visible = true;
        deathScreen.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void toMenuQuit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnRestart()
    {
        //TODO CHANGE THIS
        // GenerateDungeon();
        // Instantiate(dungeonData[0,0].roomPrefab);
        // MapManager.Instance.GoesOnTile(0,0);
        // Instantiate(playerPrefab, transform.position, Quaternion.Euler(0,45,0));
        // Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = false;
    }
}
