using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public PersistentData.WeaponTemplate weapon;
    public WeaponTemplateInfo weaponInfo;
    public TextMeshProUGUI text;
    ForgeController forgeController;

    private void Start()
    {
        forgeController = FindObjectOfType<ForgeController>();
    }

    public void LoadData()
    {
        forgeController.templateLabel.text = weaponInfo.name;
        forgeController.templateDescription.text = weaponInfo.description;
        forgeController.persistentData.equippedWeapon = weapon;
        //renderer stuff
        forgeController.UpdateWeaponStats();
        forgeController.UpdateVisuals();
        forgeController.forgeButton.gameObject.SetActive(true);
    }
}
