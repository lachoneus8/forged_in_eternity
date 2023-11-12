using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonUI : MonoBehaviour
{
    public TMP_Text damageText;
    public TMP_Text speedText;
    public TMP_Text defenseText;
    public TMP_Text recoverText;

    public TMP_Text healthText;

    public GameObject gameOverUi;

    public GameObject textPrefab;

    private PersistentData persistentData;

    private bool firstUpdate = true;

    private float gameOverTimer = 3f;

    // Update is called once per frame
    void Update()
    {
        persistentData = PersistentData.GetPersistentData();

        if (persistentData == null )
        {
            return;
        }

        if (firstUpdate )
        {
            damageText.text += " " + persistentData.GetDamage();
            speedText.text += " " + persistentData.GetSpeed();
            defenseText.text += " " + persistentData.GetDefense();
            recoverText.text += " " + persistentData.GetRecover();

            firstUpdate = false;
        }

        if (persistentData.health <= 0f)
        {
            gameOverUi.SetActive(true);
            gameOverTimer -= Time.deltaTime;

            if (gameOverTimer < 0f)
            {
                SceneManager.LoadScene("Forge");
            }
            return;
        }

        healthText.text = "Health: " + persistentData.health + "/100";
    }

    static public void DisplayText(string text, Color color, float displayLength, GameObject targetObject)
    {
        var commonUi = GameObject.FindAnyObjectByType<CommonUI>();
        commonUi.DisplayTextInt(text, color, displayLength, targetObject);
    }

    private void DisplayTextInt(string text, Color color, float displayLength, GameObject targetObject)
    {
        var textGameObject = Instantiate(textPrefab, transform);
        var worldLabel = textGameObject.GetComponent<WorldLabel>();
        worldLabel.textString = text;
        worldLabel.textColor = color;
        worldLabel.displayLength = displayLength;
        worldLabel.targetObject = targetObject;
    }
}
