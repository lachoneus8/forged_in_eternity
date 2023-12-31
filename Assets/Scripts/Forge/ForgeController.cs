using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ForgeController : MonoBehaviour
{
    public CanvasGroup forgeScreen;
    public Image startForgePanel;
    public Image startRunPanel;
    public PlayerController playerController;
    public Transform forge;
    public Transform timeBox;
    public float minDist;

    public Image introPanel;
    public CanvasGroup templateTutorial;
    public CanvasGroup alloyTutorial;
    public GameObject worldspaceTutorial;

    public CanvasGroup templateView;
    public WeaponButton weaponButton;
    public TextMeshProUGUI templateLabel;
    public TextMeshProUGUI templateDescription;

    public CanvasGroup alloyView;
    public MaterialButton materialButton;
    public RectTransform alloyListView;
    public TextMeshProUGUI ironLabel;
    public List<PersistentData.InventoryMaterial> weaponMaterials;
    public PersistentData.InventoryMaterial ironInfo;

    public WeaponAttributes curWeaponAttributes;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI recoverabilityText;

    public Button forgeButton;

    public PersistentData persistentData;
    public int yStep;
    bool firstTime = true;
    bool forged = false;
    bool isForging = false;
    // Start is called before the first frame update
    void Start()
    {
        persistentData = PersistentData.GetPersistentData();
    }


    // Update is called once per frame
    void Update()
    {
        if (persistentData == null)
        {
            persistentData = PersistentData.GetPersistentData();
            return;
        }
        else if(firstTime)
        {
            if(!persistentData.seenIntro)
            {
                introPanel.gameObject.SetActive(true);
            }
            persistentData.health = 100;
            weaponMaterials = new List<PersistentData.InventoryMaterial>();
            foreach(var material in persistentData.inventoryMaterials)
            {
                if (material.count > 0)
                {
                    weaponMaterials.Add(new PersistentData.InventoryMaterial(material.Material));
                }
            }

            int x = 170;
            int y = 190;
            for(int i=0; i<persistentData.weaponTemplates.Count; i++)
            {
                if (persistentData.weaponTemplates[i].unlocked)
                {
                    var newButton= Instantiate(weaponButton, templateView.transform);
                    newButton.weapon = persistentData.weaponTemplates[i].weaponTemplate.weaponType;
                    newButton.transform.localPosition = new Vector3(x, y);
                    newButton.text.text = persistentData.weaponTemplates[i].weaponTemplate.templateName;
                    newButton.weaponInfo = persistentData.weaponTemplates[i].weaponTemplate;
                    y -= yStep;
                }
            }

            x = 155;
            y = -45;
            int dx = 0;
            alloyListView.sizeDelta = new Vector2(alloyListView.sizeDelta.x, (weaponMaterials.Count+1)*75);

            var ironButton = Instantiate(materialButton, alloyListView);
            ironButton.transform.localPosition = new Vector2(x, y);
            ironButton.material = PersistentData.Material.iron;
            ironButton.nameLabel.text = ironInfo.Material.materialName;
            ironButton.background.color = ironInfo.Material.visualMaterial.color;
            ironButton.maxCount = ironInfo.count;
            ironButton.countLabel.text = "100 iron";
            ironLabel = ironButton.countLabel;
            foreach (var button in ironButton.buttons)
            {
                button.gameObject.SetActive(false);
            }
            y -= yStep;


            foreach (var material in persistentData.inventoryMaterials)
            {
                if (material.count > 0)
                {
                    var newButton = Instantiate(materialButton, alloyListView);
                    newButton.transform.localPosition = new Vector2(x, y);
                    newButton.material = (PersistentData.Material)dx;
                    newButton.nameLabel.text = material.Material.materialName;
                    newButton.background.color = material.Material.visualMaterial.color;
                    newButton.maxCount = material.count;

                    dx++;
                    y -= yStep;
                }

            }

            firstTime = false;
        }

        if (Vector3.Distance(playerController.transform.position, timeBox.position) < minDist&&forged&&!isForging)
        {
            startRunPanel.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("Gameplay");
            }
        }
        else
        {
            startRunPanel.gameObject.SetActive(false);
        }
        if (Vector3.Distance(playerController.transform.position, forge.position) < minDist&&!isForging&&!forged)
        {
            startForgePanel.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                forgeScreen.gameObject.SetActive(true);
                isForging = true;
            }
        }
        else
        {
            startForgePanel.gameObject.SetActive(false);
        }
    }

    public void FinishIntro()
    {
        persistentData.seenIntro = true;
        introPanel.gameObject.SetActive(false);
        templateTutorial.gameObject.SetActive(true);
        alloyTutorial.gameObject.SetActive(true);
        worldspaceTutorial.gameObject.SetActive(true);
    }
    public void ToggleView()
    {
        templateView.gameObject.SetActive(!templateView.gameObject.activeSelf);
        alloyView.gameObject.SetActive(!templateView.gameObject.activeSelf);
    }
    public void Forge()
    {
        UpdateWeaponStats();
        foreach(var weaponMaterial in weaponMaterials)
        {
            for(int i = 0; i<persistentData.inventoryMaterials.Count;i++ )
            {
                var inventoryMaterial = persistentData.inventoryMaterials[i];
                if (weaponMaterial.Material == inventoryMaterial.Material)
                {
                    inventoryMaterial.count -= (int)(weaponMaterial.count*(0.25-(persistentData.equippedWeaponAttributes.recoverability*2)));
                }
                persistentData.inventoryMaterials[i] = inventoryMaterial;
            }
        }
        persistentData.equippedWeaponAttributes = curWeaponAttributes;
        forgeScreen.gameObject.SetActive(false);
        forged = true;
        isForging = false;
    }
    public void UpdateWeaponStats()
    {
        
        //update all attributes
        float modifiedDamage = (ironInfo.Material.damage * ironInfo.count / 100.0f);
        float modifiedSpeed = (ironInfo.Material.speed * ironInfo.count / 100.0f);
        float modifiedRecoverability = (ironInfo.Material.recoverability * ironInfo.count / 100.0f);
        float modifiedDefense = (ironInfo.Material.defense * ironInfo.count / 100.0f);
        foreach (var material in weaponMaterials)
        {
            modifiedDamage += material.Material.damage * material.count / 100.0f;
            modifiedSpeed += material.Material.speed * material.count / 100.0f;
            modifiedRecoverability += material.Material.recoverability * material.count / 100.0f;
            modifiedDefense += material.Material.defense * material.count / 100.0f;
        }
        curWeaponAttributes.damage= modifiedDamage;
        curWeaponAttributes.speed = modifiedSpeed;
        curWeaponAttributes.defense = modifiedDefense;
        curWeaponAttributes.recoverability = modifiedRecoverability;

        //update the model's color as well when ready
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        var baseWeapon = persistentData.weaponTemplates[(int)persistentData.equippedWeapon].weaponTemplate;

        damageText.text = "Damage: " + baseWeapon.baseDamage + " + " + curWeaponAttributes.damage;
        speedText.text = "Speed: " + baseWeapon.baseSpeed + " + " + curWeaponAttributes.speed;
        defenseText.text = "Defense: " + baseWeapon.baseDefense + " + " + curWeaponAttributes.defense;
        recoverabilityText.text = "Recoverability: " + baseWeapon.baseRecoverability + " + " + curWeaponAttributes.recoverability;
    }
}
