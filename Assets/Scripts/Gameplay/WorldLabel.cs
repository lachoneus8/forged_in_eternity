using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldLabel : MonoBehaviour
{
    public GameObject targetObject;
    public string textString;
    public Color textColor;
    public float displayLength;

    public TMP_Text text;

    private float timeLeft;
    private float offset = 2f;
    private float offsetIncrease = 1f;

    // Start is called before the first frame update
    void Start()
    {
        text.text = textString;
        text.color = textColor;
        timeLeft = displayLength;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Destroy(gameObject);
        }
        else if (targetObject != null)
        {
            var displayPos = targetObject.transform.position;
            offset += offsetIncrease * Time.deltaTime;
            displayPos.y += offset;
            var screenPoint = Camera.main.WorldToScreenPoint(displayPos);
            text.transform.position = screenPoint;
        }
    }
}
