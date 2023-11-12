using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    public PersistentData.Material material;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI countLabel;
    public Image background;
    ForgeController forgeController;
    public List<Button> buttons;
    public int maxCount;

    private void Start()
    {
        forgeController = FindObjectOfType<ForgeController>();

        if (material != PersistentData.Material.iron)
        {
            UpdateText();
        }

    }

    public void Increment(int amt)
    {
        //decrease iron(out of 100)
        int usedAmt=Math.Min(forgeController.ironInfo.count,amt);
        usedAmt = Math.Min(usedAmt, maxCount- forgeController.weaponMaterials[(int)material].count);
        forgeController.ironInfo.count-=usedAmt;
        var modifiedMaterial = forgeController.weaponMaterials[(int)material];
        modifiedMaterial.count += usedAmt;
        forgeController.weaponMaterials[(int)material] = modifiedMaterial;

        UpdateText();
    }
    public void Decrement(int amt) {
        var modifiedMaterial = forgeController.weaponMaterials[(int)material];
        int usedAmt = Math.Min(modifiedMaterial.count, amt);
        modifiedMaterial.count -= usedAmt;
        forgeController.weaponMaterials[(int)material] = modifiedMaterial;
        forgeController.ironInfo.count += usedAmt;

        UpdateText();
    }
    public void UpdateText()
    {
        countLabel.text = forgeController.weaponMaterials[(int)material].count+"/" + maxCount;
        forgeController.ironLabel.text = forgeController.ironInfo.count+" iron";
        forgeController.UpdateWeaponStats();
    }
}
