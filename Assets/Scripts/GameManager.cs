using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int dungeonWidth;
    public int dungeonHeight;
    public RoomData[,] dungeonData;
    public enum RoomState
    {
        NotSeen,
        Seen,
        Cleared
    }
    public RoomState[,] roomState;
    public List<RoomData> possibleRooms;
    private int heightPos = 0;
    private int widthPos = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        dungeonData = new RoomData[dungeonHeight,dungeonWidth];
        roomState = new RoomState[dungeonHeight,dungeonWidth];
        GenerateDungeon();
        Instantiate(dungeonData[0,0].roomPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                dungeonData[i,j] = GetRandomRoom(); //TODO prendre en compte les portes ?
                roomState[i,j] = RoomState.NotSeen;
            }
        }
    }

    RoomData GetRandomRoom()
    {
        int k = 0;
        int index = Random.Range(0,possibleRooms.Count);
        foreach(RoomData r in possibleRooms)
        {
            if (k == index) return r;
            k++;
        }
        return null;
    }

    public void GoToNextRoom(Door.Corner corner)
    {
        switch (corner)
        {
            case Door.Corner.Up: heightPos++; break;
            case Door.Corner.Left: widthPos++; break;
            case Door.Corner.Right: widthPos--; break;
            case Door.Corner.Down: heightPos--; break;
        }
        Instantiate(dungeonData[heightPos,widthPos].roomPrefab, 50 * (Vector3.right * heightPos + Vector3.forward * widthPos), Quaternion.identity);
        RoomBehaviour.Instance.closeDoor(corner);
    }

    public bool HasNextRoom(Door.Corner corner)
    {
        switch (corner)
        {
            case Door.Corner.Up: if (heightPos+1 < dungeonHeight) return dungeonData[heightPos+1, widthPos].hasDownDoor; break;
            case Door.Corner.Left: if (widthPos+1 < dungeonWidth) return dungeonData[heightPos, widthPos+1].hasRightDoor; break;
            case Door.Corner.Right: if (widthPos > 0) return dungeonData[heightPos, widthPos-1].hasLeftDoor; break;
            case Door.Corner.Down: if (heightPos > 0) return dungeonData[heightPos-1, widthPos].hasUpDoor; break;
        }
        return false;
    }

    public RoomData GetCurrentRoomData()
    {
        return dungeonData[heightPos,widthPos];
    }

    public bool isCurrentRoomClear()
    {
        return roomState[heightPos,widthPos] == RoomState.Cleared;
    }

    public void seenRoom()
    {
        roomState[heightPos,widthPos] = RoomState.Seen;
    }

    public void clearedRoom()
    {
        roomState[heightPos,widthPos] = RoomState.Cleared;
    }
}
