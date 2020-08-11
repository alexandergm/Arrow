using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    
    public GameObject camera;
    public GameObject obj;


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
           
            camera.GetComponent<CameraControl>().enabled = false;
            Destroy(obj);

        }
    }
}