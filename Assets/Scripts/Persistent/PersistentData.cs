using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentData : MonoBehaviour
{
    private static bool isLoading = false;
    private static bool markedDontDestroy = false;

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
}
