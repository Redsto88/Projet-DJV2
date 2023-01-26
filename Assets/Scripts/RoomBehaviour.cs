using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public static RoomBehaviour Instance;
    [SerializeField] private GameObject doorUp;
    [SerializeField] private GameObject doorLeft;
    [SerializeField] private GameObject doorRight;
    [SerializeField] private GameObject doorDown;
    [SerializeField] private Array2D<ObjData> enemyWaves;
    public int enemiesLeft = 0;
    public int waveNumber = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CountEnemyDeath();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {enemiesLeft = 0; CountEnemyDeath();}
        // if (enemiesLeft == 0) //TODO use CountEnemyDeath when an enemy dies
        // {
        //     if (waveNumber == enemyWaves.arrays.Count || GameManager.Instance.GetCurrentRoomData().isDiscovered)
        //     {
        //         if (doorUp != null && GameManager.Instance.HasNextRoom(Door.Corner.Up)) doorUp.GetComponent<Door>().isOpen = true;
        //         if (doorLeft != null && GameManager.Instance.HasNextRoom(Door.Corner.Left)) doorLeft.GetComponent<Door>().isOpen = true;
        //         if (doorRight != null && GameManager.Instance.HasNextRoom(Door.Corner.Right)) doorRight.GetComponent<Door>().isOpen = true;
        //         if (doorDown != null && GameManager.Instance.HasNextRoom(Door.Corner.Down)) doorDown.GetComponent<Door>().isOpen = true;
        //     }
        //     else NextEnemyWave();
        // }
    }

    public IEnumerator useDoor(Door.Corner corner)
    {
        PlayerController.Instance.characterController.enabled = false;
        yield return null;
        //TODO animation of using door
        GameManager.Instance.GoToNextRoom(corner);
    }

    public void closeDoor(Door.Corner corner)
    {
        switch (corner)
        {
            case Door.Corner.Up: Debug.Log(doorDown.transform.position + Vector3.right * 2); PlayerController.Instance.transform.position = doorDown.transform.position + Vector3.right * 4; break;
            case Door.Corner.Left: PlayerController.Instance.transform.position = doorRight.transform.position + Vector3.forward * 4; break;
            case Door.Corner.Right: PlayerController.Instance.transform.position = doorLeft.transform.position - Vector3.forward * 4; break;
            case Door.Corner.Down: Debug.Log(doorUp.transform.position - Vector3.right * 2); PlayerController.Instance.transform.position = doorUp.transform.position - Vector3.right * 4; break;
        }
        PlayerController.Instance.characterController.enabled = true;
    }

    void NextEnemyWave()
    {
        GameManager.Instance.seenRoom();
        var wave = enemyWaves.arrays[waveNumber];
        enemiesLeft = wave.cells.Count;
        foreach (ObjData obj in wave.cells)
        {
            Instantiate(obj.prefab, obj.position + transform.position, obj.rotation);
        }
        waveNumber++;
    }
    
    void CountEnemyDeath()
    {
        enemiesLeft--;
        if (enemiesLeft <= 0)
        {
            if (waveNumber == enemyWaves.arrays.Count || GameManager.Instance.isCurrentRoomClear())
            {
                GameManager.Instance.clearedRoom();
                Debug.Log(GameManager.Instance.HasNextRoom(Door.Corner.Up));
                if (doorUp != null && GameManager.Instance.HasNextRoom(Door.Corner.Up)) doorUp.GetComponent<Door>().isOpen = true;
                if (doorLeft != null && GameManager.Instance.HasNextRoom(Door.Corner.Left)) doorLeft.GetComponent<Door>().isOpen = true;
                if (doorRight != null && GameManager.Instance.HasNextRoom(Door.Corner.Right)) doorRight.GetComponent<Door>().isOpen = true;
                if (doorDown != null && GameManager.Instance.HasNextRoom(Door.Corner.Down)) doorDown.GetComponent<Door>().isOpen = true;
            }
            else NextEnemyWave();
        }
    }
}
