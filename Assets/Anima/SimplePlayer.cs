using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour
{
    
    //public GameObject obj;
    public GameObject rt;
    //public GameObject camera;
    public ragdoll calldie;
    //public GameObject child;
    //public Transform parent;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //public void Example(Transform newParent)
    //   {
    //       // Sets "newParent" as the new parent of the child GameObject.
    //       child.transform.SetParent(newParent);
   
        
    //   }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
         {

            //    rt.GetComponent<Fly>().enabled = false;
            //Example(parent);
            Destroy(rt);
            calldie.die();
            
            //print(obj.gameObject.name);
            //obj.GetComponent<Animator>().SetBool("Bool", true);
            //obj.GetComponent<Animator>().enabled = false;
            //obj.GetComponent<Rigidbody>().AddForce((Vector3.forward) * 1);
            //rt.GetComponent<MeshRenderer>().enabled = false;
            //camera.GetComponent<CameraControl>().enabled = false;

        }
    }
}