using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        earthAdamantium
    }
    [Serializable]
    public struct InventoryMaterial
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
    public struct PlayerWeaponTemplate
    {
        public bool unlocked;
        public WeaponTemplateInfo weaponTemplate;
    }

    public void PlayerDamage(float damage)
    {
        health -= damage * (1 - equippedWeaponAttributes.defense);
    }
}
