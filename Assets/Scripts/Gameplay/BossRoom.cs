using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : MonoBehaviour
{
    public Image victoryPanel;
    public Zone.ZoneChoice nextZone;
    public PlayerController playerController;
    public Boss boss;
    
    private PersistentData persistent;

    private void Start()
    {
        persistent = FindObjectOfType<PersistentData>();
    }

    public void bossDeath()
    {
        persistent.curZone = nextZone;
        victoryPanel.gameObject.SetActive(true);
    }
    public void NextZone()
    {
        if (nextZone == Zone.ZoneChoice.Zone2)
        {
            SceneManager.LoadScene("Gameplay");
        }
        else if(nextZone == Zone.ZoneChoice.Zone3)
        {
            SceneManager.LoadScene("Credits");
        }
    }

    private void Update()
    {
        if (persistent.health <= 0f)
        {
            return;
        }

        if (boss != null)
        {
            var diff = boss.transform.position - playerController.transform.position;
            Debug.Log(diff.magnitude.ToString());
            if (diff.magnitude < 3.0f)
            {
                playerController.HandleAttack(persistent, boss.gameObject);
            }
        }
    }
}
