using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float templatePickupRange;
    public float gatheringPointRange;
    public float attackRange;

    private PersistentData persistentData;
    private Zone curZone;

    private List<GameObject> spawnedList = new List<GameObject>();

    private bool firstUpdate = true;

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

            return;
        }

        if (firstUpdate)
        {
            curZone = GetZone(persistentData.curZone);
            firstUpdate = false;
        }

        if (persistentData.health <= 0f)
        {
            return;
        }

        var diff = curRoom.exitPoint.transform.position - player.transform.position;
        if (diff.magnitude < distanceToExit && CanLeaveRoom())
        {
            player.skipUpdate = true;

            ChangeRoom();
            return;
        }

        bool hasEnemies = false;
        GatheringPoint gatheringPointNearby = null;
        Enemy nearestEnemyInReach = null;
        float closestEnemyDistance = float.MaxValue;

        foreach (var spawnable in spawnedList)
        {
            if (spawnable == null)
            {
                continue;
            }

            // If player is close to template, pick it up
            if (spawnable.GetComponent<Template>() != null)
            {
                var templateDiff = spawnable.transform.position - player.transform.position;
                if (templateDiff.magnitude < templatePickupRange)
                {
                    PickupTemplate(spawnable);
                }
            }
            else if (spawnable.GetComponent<GatheringPoint>() != null)
            {
                var gatheringPointDiff = spawnable.transform.position - player.transform.position;
                if (gatheringPointDiff.magnitude < gatheringPointRange)
                {
                    gatheringPointNearby = spawnable.GetComponent<GatheringPoint>();
                }
            }
            else if (spawnable.GetComponent<Enemy>() != null)
            {
                hasEnemies = true;

                var enemyDiff = spawnable.transform.position - player.transform.position;
                if (enemyDiff.magnitude < attackRange && enemyDiff.magnitude < closestEnemyDistance)
                {
                    closestEnemyDistance = enemyDiff.magnitude;
                    nearestEnemyInReach = spawnable.GetComponent<Enemy>();
                }
            }
        }

        if (gatheringPointNearby != null && !hasEnemies)
        {
            HandleGathering(gatheringPointNearby);
        }
        else if (nearestEnemyInReach != null)
        {
            player.HandleAttack(persistentData, nearestEnemyInReach.gameObject);
        }
    }

    private bool CanLeaveRoom()
    {
        // Don't allow to proceed if there is a spawned template
        foreach (var spawned in spawnedList)
        {
            if (spawned != null)
            {
                var template = spawned.GetComponent<Template>();
                if (template != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void PickupTemplate(GameObject spawnable)
    {
        var unlockableTemplates = new List<PersistentData.WeaponTemplate>();
        foreach (var weaponTemplate in persistentData.weaponTemplates)
        {
            if (!weaponTemplate.unlocked)
            {
                unlockableTemplates.Add(weaponTemplate.weaponTemplate.weaponType);
            }
        }
        
        if (unlockableTemplates.Count == 0)
        {
            Destroy(spawnable);
            return;
        }

        var unlockableIndex = UnityEngine.Random.Range(0, unlockableTemplates.Count);
        var unlockedType = unlockableTemplates[unlockableIndex];

        int indexToUnlock = -1;
        for (int i = 0; i < persistentData.weaponTemplates.Count; ++i)
        {
            if (persistentData.weaponTemplates[i].weaponTemplate.weaponType == unlockedType)
            {
                indexToUnlock = i;
                break;
            }
        }

        CommonUI.DisplayText("Unlocked template: " + persistentData.weaponTemplates[indexToUnlock].weaponTemplate.templateName, 
            Color.green, 4f, player.gameObject);

        persistentData.weaponTemplates[indexToUnlock].unlocked = true;

        Destroy(spawnable);
    }

    private void HandleGathering(GatheringPoint gatheringPoint)
    {
        if (Input.GetMouseButtonDown(0))
        {
            int numGathered = gatheringPoint.HandleHit();
            if (numGathered > 0)
            {
                persistentData.AddMaterial(gatheringPoint.material, numGathered);
            }

            var materialName = persistentData.GetMaterialName(gatheringPoint.material);
            CommonUI.DisplayText("+ " + numGathered + " " + materialName, numGathered > 0 ? Color.blue : Color.yellow, 3f, player.gameObject);
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
        foreach (var spawned in spawnedList)
        {
            if (spawned != null)
            {
                Destroy(spawned.gameObject);
            }
        }
        spawnedList.Clear();

        var roomType = curZone.GetRoomType(persistentData);

        if (roomType == Zone.RoomType.ZoneBoss)
        {
            switch (persistentData.curZone)
            {
                case Zone.ZoneChoice.Zone1:
                    SceneManager.LoadScene("ZoneBoss 1");
                    break;
                case Zone.ZoneChoice.Zone2:
                    SceneManager.LoadScene("ZoneBoss 2");
                    break;
            }
            
            return;
        }
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

            var spawnedGameObject = Instantiate(prefab, position, Quaternion.identity, spawnableParent);
            // Hack to get at specific different spawnable types
            ASpawnable spawnable = spawnedGameObject.GetComponent<Template>();
            if (spawnable == null) spawnable = spawnedGameObject.GetComponent<GatheringPoint>();
            if (spawnable == null) spawnable = spawnedGameObject.GetComponent<Enemy>();
            CommonUI.DisplayText(spawnable.GetSpawnText(), spawnable.GetSpawnTextColor(), 5f, spawnedGameObject);

            spawnedList.Add(spawnedGameObject);
        }
    }
}
