using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour

{
    public GameObject obj;
    public float movementSpeed = 100f;
    public float resetSpeed = 100f;
    public float shiftSpeed = 150f;
    public float controlSpeed = 50f;

    public float horizSensivity = 2f;
    public float vertSensivity = 2f;

    public float yaw = 0f;
    public float pitch = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
        
    }

    private void Update()
    {
        yaw += horizSensivity * Input.GetAxis("Mouse X");
        pitch -= horizSensivity * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 1.418f);
        
        

        //transform.localPosition += transform.forward * Time.deltaTime * controlSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = shiftSpeed;
        }
        else
        {
            movementSpeed = resetSpeed;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += transform.forward * Time.deltaTime * controlSpeed;
            Destroy(obj);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition -= transform.right * Time.deltaTime * controlSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
           transform.localPosition -= transform.forward * Time.deltaTime * controlSpeed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += transform.right * Time.deltaTime * controlSpeed;
        }
    }
}
