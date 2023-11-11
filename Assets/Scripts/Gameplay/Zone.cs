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

    public ZoneChoice zoneChoice;

    private int numRoomsCreated;

    public void GetRoomType()
    {
        
    }
}
