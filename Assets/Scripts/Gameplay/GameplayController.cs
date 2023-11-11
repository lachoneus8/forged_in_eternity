using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public List<Room> roomList;
    public List<Zone> zoneList;

    public Room curRoom;
    public PlayerController player;
    public float distanceToExit;

    private PersistentData persistentData;
    private Zone curZone;

    // Start is called before the first frame update
    void Start()
    {
        persistentData = PersistentData.GetPersistentData();
    }

    // Update is called once per frame
    void Update()
    {
        if (persistentData == null)
        {
            persistentData = PersistentData.GetPersistentData();

            if (persistentData != null )
            {
                curZone = GetZone(persistentData.curZone);
            }
            return;
        }

        var diff = curRoom.exitPoint.transform.position - player.transform.position;
        if (diff.magnitude < distanceToExit)
        {
            ChangeRoom();
            return;
        }
    }

    private Zone GetZone(Zone.ZoneChoice curZone)
    {
        foreach (var zone in zoneList)
        {
            if (zone.zoneChoice == curZone)
            {
                return zone;
            }
        }

        Debug.LogError("Could not find zone!");
        return null;
    }

    private void ChangeRoom()
    {
        curZone.GetRoomType();

        foreach (var room in roomList)
        {

        }
    }
}
