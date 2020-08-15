using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour
{
    
    public GameObject obj;
    public GameObject rt;
    public GameObject camera;


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
            print(obj.gameObject.name);
            obj.GetComponent<Animator>().SetBool("Bool", true);
            //obj.GetComponent<Animator>().enabled = false;
            //obj.GetComponent<Rigidbody>().AddForce((Vector3.forward) * 1);
            //rt.GetComponent<MeshRenderer>().enabled = false;
            camera.GetComponent<CameraControl>().enabled = false;
            Destroy(rt);            
        }
    }
}