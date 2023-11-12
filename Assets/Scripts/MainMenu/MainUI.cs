using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    private PersistentData persistentData;

    private void Update()
    {
        if (persistentData == null)
        {
            persistentData = PersistentData.GetPersistentData();
        }
    }

    public void GoToForge()
    {
        SceneManager.LoadScene("Forge");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
