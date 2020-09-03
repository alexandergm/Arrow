using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour

{
    public GameObject obj;
    //public GameObject arrow;
    public float movementSpeed = 100f;
    public float resetSpeed = 100f;
    public float shiftSpeed = 150f;
    public float controlSpeed = 50f;

    public float horizSensivity = 2f;
    public float vertSensivity = 2f;

    public float yaw = 0f;
    public float pitch = 0f;
    //public  float forceImpule = 10000f;
    //Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
        
        
    }

    void Update()
    {
        yaw += horizSensivity * Input.GetAxis("Mouse X");
        pitch -= horizSensivity * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, -10.375f);
        
        

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
            gameObject.GetComponent<MeshRenderer>().enabled = true;
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
    //public void AddImpulse()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    rb.AddForce(arrow.transform.localPosition * forceImpule, ForceMode.Impulse);
    //}
}
