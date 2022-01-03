using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Sets mouse sensitivity to 100
    public float mouseSensitivity = 100f;

    //Sets playerbody to be changed
    public Transform playerBody;

    //Sets xRotation to 0
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //Locks cursor to screen border
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Camera movement multipled by mouse sensitivity and time since last update (so higher fps doesn't increase turning);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Sets XRotation to be the negative of mouseY (positive is inverted);
        xRotation -= mouseY;

        //Sets rotation to be within -90 and 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotation transformation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //Rotates
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
