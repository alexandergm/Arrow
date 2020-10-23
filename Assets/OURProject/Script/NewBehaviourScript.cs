using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    
   // public GameObject camera;
    public GameObject obj;
   // public GameObject finalCamera;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(obj);
            //сamera.GetComponent<Camera>().enabled = false;
         //   finalCamera.GetComponent<Camera>().enabled = true;


        }
    }
}