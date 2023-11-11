using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForgeController : MonoBehaviour
{

    public CanvasGroup templateView;
    public WeaponButton weaponButton;
    public TextMeshProUGUI templateLabel;
    public TextMeshProUGUI templateDescription;

    public CanvasGroup alloyView;
    public PersistentData persistentData;
    public int yStep;
    bool firstTime = true;
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
        if(firstTime)
        {
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
            firstTime = false;
        }
    }


    public void ToggleView()
    {
        templateView.gameObject.SetActive(!templateView.gameObject.activeSelf);
        alloyView.gameObject.SetActive(!templateView.gameObject.activeSelf);
    }
}
