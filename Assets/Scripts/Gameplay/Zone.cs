using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public enum ZoneChoice
    {
        Zone1, // Intro zone, all templates, steel
        Zone2, // 2nd zone, sub-boss, tier 1 mats
        Zone3  // Final zone, final boss, tier 2 mats
    }

    public enum RoomType
    {
        Level0Enemies,
        Level1Enemies,
        Level2Enemies,
        Level3Enemies,
        Level4Enemies,
        Level5Enemies,
        Template,
        BasicTier1,
        BasicTier2, 
        SpecializedTier1,
        SpecializedTier2,
        MiniBossZone1,
        MiniBossZone2,
        ZoneBoss
    }

    public ZoneChoice zoneChoice;

    public List<RoomType> roomsBeforeMiniBoss;
    public List<RoomType> roomsAfterMiniBoss;

    public int numRoomsBeforeMiniBoss;
    public int numRoomsBeforeBoss;

    private int numRoomsCreated = 0;
    private bool miniBossGiven;

    public RoomType GetRoomType(PersistentData persistentData)
    {
        numRoomsCreated++;
        if (!miniBossGiven && CheckForMiniBoss(persistentData))
        {
            miniBossGiven = true;
            return zoneChoice == ZoneChoice.Zone1 ? RoomType.MiniBossZone1 : RoomType.MiniBossZone2;
        }

        if (CheckForZoneBoss(persistentData))
        {
            return RoomType.ZoneBoss;
        }

        RoomType ret;
        if (miniBossGiven)
        {
            ret = GetRandomRoomType(persistentData, roomsAfterMiniBoss);
        }
        else
        {
            ret = GetRandomRoomType(persistentData, roomsBeforeMiniBoss);
        }

        return ret;
    }

    private bool CheckForMiniBoss(PersistentData persistentData)
    {
        if (zoneChoice == ZoneChoice.Zone1)
        {
            if (persistentData.GetTemplatesLocked() < 4 && numRoomsCreated > numRoomsBeforeMiniBoss)
            {
                return true;
            }
        }
        else
        {
            if (numRoomsCreated > numRoomsBeforeMiniBoss)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckForZoneBoss(PersistentData persistentData)
    {
        if (zoneChoice == ZoneChoice.Zone1)
        {
            if (persistentData.GetTemplatesLocked() == 0 && numRoomsCreated > numRoomsBeforeBoss)
            {
                return true;
            }
        }
        else
        {
            if (numRoomsCreated > numRoomsBeforeBoss)
            {
                return true;
            }
        }

        return false;
    }

    private RoomType GetRandomRoomType(PersistentData persistentData, List<RoomType> rooms)
    {
        List<RoomType> legalRoomTypes = new List<RoomType>();
        foreach (RoomType roomType in rooms)
        {
            if (roomType != RoomType.Template || persistentData.GetTemplatesLocked() != 0)
            {
                legalRoomTypes.Add(roomType);
            }
        }

        var roomIndex = UnityEngine.Random.Range(0, legalRoomTypes.Count);
        return legalRoomTypes[roomIndex];
    }
}
