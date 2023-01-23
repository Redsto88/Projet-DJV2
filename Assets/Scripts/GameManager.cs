using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int dungeonWidth;
    public int dungeonHeight;
    public RoomData[,] dungeonData;
    public List<RoomData> possibleRooms;

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
        GenerateDungeon();
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
}
