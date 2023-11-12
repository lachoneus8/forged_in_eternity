using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public PlayerController playerController;
    public float zOffset;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = playerController.transform.position;
        var curPos = transform.position;
        curPos.x = playerPos.x;
        curPos.z = playerPos.z + zOffset;
        transform.position = curPos;
    }
}
