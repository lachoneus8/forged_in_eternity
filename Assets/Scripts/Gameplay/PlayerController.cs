using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public int groundLayer;
    public float playerSpeed = 2.0f;

    public bool skipUpdate = false;


    private int layerAsMask;

    // Start is called before the first frame update
    void Start()
    {
        layerAsMask = 1 << groundLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (skipUpdate)
        {
            skipUpdate = false;
            return;
        }

        controller.enabled = true;

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

        // Correct x rotation
        var curRot = transform.rotation.eulerAngles.y;
        var updatedRot = new Vector3(0, curRot, 0);
        transform.rotation = Quaternion.Euler(updatedRot);
    }
}
