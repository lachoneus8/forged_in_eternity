using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeController : MonoBehaviour
{

    public CanvasGroup templateView;
    public CanvasGroup alloyView;
    // Start is called before the first frame update
    void Start()
    {
        //load data from persistent

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleView()
    {
        templateView.gameObject.SetActive(!templateView.gameObject.activeSelf);
        alloyView.gameObject.SetActive(!templateView.gameObject.activeSelf);
    }
}
