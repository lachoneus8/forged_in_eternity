using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringPoint : ASpawnable
{
    public PersistentData.Material material;
    public int amountRemaining;

    private int gatherPerHit = 3;

    public override string GetSpawnText()
    {
        return "Click to mine me";
    }

    public override Color GetSpawnTextColor()
    {
        return Color.cyan;
    }

    public int HandleHit()
    {
        if (amountRemaining == 0)
        {
            return 0;
        }

        int amountToReturn = 0;
        if (amountRemaining < gatherPerHit)
        {
            amountToReturn = amountRemaining;
            amountRemaining = 0;
        }
        else
        {
            amountToReturn = gatherPerHit;
            amountRemaining -= gatherPerHit;
        }

        return amountToReturn;
    }

    // Start is called before the first frame update
    void Start()
    {
        amountRemaining = UnityEngine.Random.Range(10, 20);
    }

}
