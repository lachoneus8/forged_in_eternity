using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponTemplateInfo", menuName ="Forged/WeaponTemplateInfo")]
public class WeaponTemplateInfo : ScriptableObject
{
    public string templateName;
    public string description;
    public PersistentData.WeaponTemplate weaponType;
    public GameObject model;
}
