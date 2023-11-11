using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public void GoToForge()
    {
        SceneManager.LoadScene("Forge");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
