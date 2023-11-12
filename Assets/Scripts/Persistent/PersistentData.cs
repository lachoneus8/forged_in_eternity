using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Terrain;

public class PersistentData : MonoBehaviour
{
    private static bool isLoading = false;
    private static bool markedDontDestroy = false;
    public List<PlayerWeaponTemplate> weaponTemplates;
    public List<InventoryMaterial> inventoryMaterials;
    public WeaponTemplate equippedWeapon;
    public WeaponAttributes equippedWeaponAttributes;
    public Zone.ZoneChoice curZone = Zone.ZoneChoice.Zone1;
    public float health;

    public bool seenIntro=false;
    public static PersistentData GetPersistentData()
    {
        var persistentData = FindObjectOfType<PersistentData>();

        if (persistentData == null && !isLoading)
        {
            SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
            persistentData = FindObjectOfType<PersistentData>();
            isLoading = true;
        }

        if (persistentData != null && !markedDontDestroy)
        {
            GameObject.DontDestroyOnLoad(persistentData.gameObject);
        }

        return persistentData;
    }

    public enum WeaponTemplate
    {
        dagger,
        longsword,
        greatAxe,
        rapier,
        halberd,
        handAxe
    }
    public enum Material
    {
        steel,
        draconium,
        mithril,
        aquanite,
        adamantium,
        blacksteel,
        fireDraconium,
        airMithril,
        waterAquanite,
        earthAdamantium,
        iron
    }
    [Serializable]
    public class InventoryMaterial
    {
        public MaterialInfo Material;
        public int count;
        public InventoryMaterial(MaterialInfo baseInfo)
        {
            count = 0;
            Material = baseInfo;
        }
    }
    [Serializable]
    public class PlayerWeaponTemplate
    {
        public bool unlocked;
        public WeaponTemplateInfo weaponTemplate;
    }

    public void PlayerDamage(float damage)
    {
        health -= damage * (1 - GetDefense() * .1f);
    }

    public void AddMaterial(Material materialType, int numGathered)
    {
        foreach (var material in inventoryMaterials)
        {
            if (material.Material.materialType == materialType)
            {
                material.count += numGathered;
                break;
            }
        }
    }

    public string GetMaterialName(Material materialType)
    {
        foreach (var material in inventoryMaterials)
        {
            if (material.Material.materialType == materialType)
            {
                return material.Material.materialName;
            }
        }
        return "";
    }

    public int GetTemplatesLocked()
    {
        int lockedCount = 0;
        foreach (var template in weaponTemplates)
        {
            if (!template.unlocked)
            {
                lockedCount++;
            }
        }
        return lockedCount;
    }

    public PlayerWeaponTemplate GetWeapon(WeaponTemplate weaponType)
    {
        foreach (var weapon in weaponTemplates)
        {
            if (weapon.weaponTemplate.weaponType == weaponType)
            {
                return weapon;
            }
        }

        return null;
    }

    public float GetDamage()
    {
        var weaponTemplate = GetWeapon(equippedWeapon);
        return weaponTemplate.weaponTemplate.baseDamage + equippedWeaponAttributes.damage;
    }

    internal float GetSpeed()
    {
        var weaponTemplate = GetWeapon(equippedWeapon);
        return weaponTemplate.weaponTemplate.baseSpeed + equippedWeaponAttributes.speed;
    }

    internal float GetDefense()
    {
        var weaponTemplate = GetWeapon(equippedWeapon);
        return weaponTemplate.weaponTemplate.baseDefense + equippedWeaponAttributes.defense;
    }

    internal float GetRecover()
    {
        var weaponTemplate = GetWeapon(equippedWeapon);
        return weaponTemplate.weaponTemplate.baseRecoverability + equippedWeaponAttributes.recoverability;
    }
}
