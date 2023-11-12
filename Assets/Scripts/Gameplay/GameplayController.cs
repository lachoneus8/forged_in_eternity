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

    public List<SpawnRecord> spawnRecords;

    public Rect spawnAreaXZ;

    public Transform spawnableParent;

    private PersistentData persistentData;
    private Zone curZone;

    private List<GameObject> spawnedList = new List<GameObject>();

    [Serializable]
    public struct SpawnRecord
    {
        public Zone.RoomType roomType;
        public List<GameObject> spawnablePrefabs;
    }

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
            player.skipUpdate = true;

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
        var roomType = curZone.GetRoomType();
        Debug.Log("New room: " + roomType.ToString());

        var legalList = new List<Room>();

        foreach (var room in roomList)
        {
            if (room.entryDir == curRoom.exitDir)
            {
                legalList.Add(room);
            }
        }

        var roomIndex = UnityEngine.Random.Range(0, legalList.Count);
        var newRoom = legalList[roomIndex];

        Destroy(curRoom.gameObject);
        curRoom = Instantiate(newRoom).GetComponent<Room>();

        player.transform.position = curRoom.entryPoint.transform.position;
        player.skipUpdate = true;
        player.controller.enabled = false;

        var spawnables = GetSpawnables(roomType);
        var spawnableIndex = UnityEngine.Random.Range(0, spawnables.Count);
        if (spawnables.Count > 0)
        {
            var selectedSpawnable = spawnables[spawnableIndex];

            InstantiateSpawnables(selectedSpawnable);
        }
    }

    private List<SpawnRecord> GetSpawnables(Zone.RoomType roomType)
    {
        var spawnables = new List<SpawnRecord>();
        foreach (var spawnable in spawnRecords)
        {
            if (spawnable.roomType == roomType)
            {
                spawnables.Add(spawnable);
            }
        }

        return spawnables;
    }

    private void InstantiateSpawnables(SpawnRecord selectedSpawnable)
    {
        foreach (var prefab in selectedSpawnable.spawnablePrefabs)
        {
            var position = new Vector3();
            position.x = UnityEngine.Random.Range(spawnAreaXZ.xMin, spawnAreaXZ.xMax);
            position.z = UnityEngine.Random.Range(spawnAreaXZ.yMin, spawnAreaXZ.yMax);
            position.y = prefab.transform.position.y;

            spawnedList.Add(Instantiate(prefab, position, Quaternion.identity, spawnableParent));
        }
    }
}
