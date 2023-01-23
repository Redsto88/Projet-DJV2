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
    [SerializeField] private Array2D<GameObject> enemyWaves;
    public int enemiesLeft;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
