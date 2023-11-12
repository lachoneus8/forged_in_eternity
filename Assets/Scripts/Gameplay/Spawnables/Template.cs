using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : ASpawnable
{
    public override string GetSpawnText()
    {
        return "Pick me up to acquire a new type of weapon to forge!";
    }

    public override Color GetSpawnTextColor()
    {
        return Color.green;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
