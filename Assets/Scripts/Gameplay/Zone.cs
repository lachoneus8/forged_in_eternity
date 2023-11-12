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
        MiniBoss,
        ZoneBoss
    }

    public ZoneChoice zoneChoice;

    public List<RoomType> roomsBeforeMiniBoss;
    public List<RoomType> roomsAfterMiniBoss;

    public int numRoomsBeforeMiniBoss;
    public int numRoomsBeforeBoss;

    private int numRoomsCreated = 0;
    private int numTemplatesGiven = 0;
    private bool miniBossGiven;

    public RoomType GetRoomType()
    {
        numRoomsCreated++;
        if (!miniBossGiven && CheckForMiniBoss())
        {
            miniBossGiven = true;
            return RoomType.MiniBoss;
        }

        if (CheckForZoneBoss())
        {
            return RoomType.ZoneBoss;
        }

        RoomType ret;
        if (miniBossGiven)
        {
            ret = GetRandomRoomType(roomsAfterMiniBoss);
        }
        else
        {
            ret = GetRandomRoomType(roomsBeforeMiniBoss);
        }

        if (ret == RoomType.Template)
        {
            numTemplatesGiven++;
        }

        return ret;
    }

    private bool CheckForMiniBoss()
    {
        if (zoneChoice == ZoneChoice.Zone1)
        {
            if (numTemplatesGiven == 2)
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

    private bool CheckForZoneBoss()
    {
        if (zoneChoice == ZoneChoice.Zone1)
        {
            if (numTemplatesGiven == 5)
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

    private RoomType GetRandomRoomType(List<RoomType> rooms)
    {
        var roomIndex = UnityEngine.Random.Range(0, rooms.Count);
        return rooms[roomIndex];
    }
}
