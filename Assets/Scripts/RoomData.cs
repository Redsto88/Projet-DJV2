using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Room", menuName = "Game/Room")]
public class RoomData : ScriptableObject
{
    public GameObject roomPrefab;
    public bool isDiscovered;
    public bool isDestroyed;
    public bool hasUpDoor;
    public bool hasLeftDoor;
    public bool hasRightDoor;
    public bool hasDownDoor;
}
