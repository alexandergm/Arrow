using UnityEngine;
using System.Collections;

public class SimplePlayer : MonoBehaviour
{
    
    public GameObject obj;
    public GameObject rt;
    //public GameObject camera;
    public ragdoll calldie;
    public GameObject star;
    public float force = 10000;
   
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
            star.SetActive(true);
            //Destroy(rt);
            calldie.die();
            //obj.GetComponent<Rigidbody>().AddForce(rt.transform.TransformDirection(Vector3.forward) * force);
            

            //print(obj.gameObject.name);
            //obj.GetComponent<Animator>().SetBool("Bool", true);
            //obj.GetComponent<Animator>().enabled = false;
            obj.GetComponent<Rigidbody>().AddForce((Vector3.forward) * force);
            //rt.SetActive(false);

            //camera.GetComponent<CameraControl>().enabled = false;

        }
    }
}