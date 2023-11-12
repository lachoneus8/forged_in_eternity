using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ASpawnable
{
    public List<string> tauntMessages;

    public override string GetSpawnText()
    {
        var tauntIndex = UnityEngine.Random.Range(0, tauntMessages.Count);
        return tauntMessages[tauntIndex];
    }

    public override Color GetSpawnTextColor()
    {
        return Color.magenta;
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
