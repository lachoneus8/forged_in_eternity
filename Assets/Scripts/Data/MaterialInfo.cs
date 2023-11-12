using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MaterialInfo", menuName ="Forged/MaterialInfo")]
public class MaterialInfo : ScriptableObject
{
    public string materialName;
    public PersistentData.Material materialType;
    public int damage;
    public int speed;
    public int defense;
    public int recoverability;
    public Material visualMaterial;
}
