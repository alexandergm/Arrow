using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SimplePlayer : MonoBehaviour
{
    
    public GameObject obj;
    public GameObject arrow;
    public GameObject rt;
    public GameObject scr1;
    //public GameObject camera;
    public ragdoll calldie;
    public GameObject star;
    public float forceImpuse = 10000;
    public float x = 2;
    public float y = 2;
    public float z = 2;

    Rigidbody rb;
   
    //public GameObject child;
    //public Transform parent;
   
    private GameObject scr2;


    void Start()
    {
        //scr1 = GameObject.Find("SM_Prop_Arrow_01").GetComponent<Fly>();
        
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
            //obj.GetComponent<Animator>().SetBool("Bool", true);
            Destroy(rt);
            Time.timeScale = 0.4f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            obj.GetComponent<Transform>().localScale = new Vector3(x, y, z);
            calldie.die();
            //AddImpulse();
            


            //obj.GetComponent<Rigidbody>().AddForce(rt.transform.TransformDirection(Vector3.forward) * force);
            

            //print(obj.gameObject.name);
            
            //obj.GetComponent<Animator>().enabled = false;
            //obj.GetComponent<Rigidbody>().AddForce((Vector3.forward) * force);
            //rt.SetActive(false);

            //camera.GetComponent<CameraControl>().enabled = false;

        }
    }

    public void AddImpulse()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(arrow.transform.localPosition * forceImpuse, ForceMode.Impulse);
    }
}