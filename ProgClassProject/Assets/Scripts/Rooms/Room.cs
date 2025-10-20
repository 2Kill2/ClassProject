using System.Xml.Serialization;
using UnityEngine;

public abstract class Room
{
    public virtual void EnterRoom()
    {
        Debug.Log("You enter a room.");
    }

    public GameObject GetRoomPrefab(GameObject treasureRoomPrefab, GameObject safeRoomPrefab, GameObject encounterRoomPrefab)
    {
        if (this is TreasureRoom) return treasureRoomPrefab;
        else if (this is SafeRoom) return safeRoomPrefab;
        else if (this is EncounterRoom) return encounterRoomPrefab;
        else return null;
    }
}
