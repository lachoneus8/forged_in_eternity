using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Direction
    {
        North,
        East,
        West,
        South,
        None
    }

    public GameObject entryPoint;
    public GameObject exitPoint;

    public Direction entryDir;
    public Direction exitDir;
}
