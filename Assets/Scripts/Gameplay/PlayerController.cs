using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public int groundLayer;
    public float playerSpeed = 2.0f;

    private int layerAsMask;

    // Start is called before the first frame update
    void Start()
    {
        layerAsMask = 1 << groundLayer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 100f, layerAsMask))
        {
            var lookAt = hitInfo.point;
            lookAt.y = 1;
            gameObject.transform.LookAt(lookAt);
        }
    }
}
