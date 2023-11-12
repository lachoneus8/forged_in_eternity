using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASpawnable : MonoBehaviour
{
    public Zone.RoomType roomType;

    public abstract string GetSpawnText();
    public abstract Color GetSpawnTextColor();
}
