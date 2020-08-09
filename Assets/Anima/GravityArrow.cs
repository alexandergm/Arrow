using UnityEngine;
using System.Collections;

public class GravityArrow : MonoBehaviour
{

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
            //obj.GetComponent<Rigidbody>().useGravity = true;
            //obj.GetComponent<Rigidbody>().isKinematic = false;
            obj.GetComponent<Rigidbody>().freezeRotation = true;
            obj.GetComponent<Fly>().enabled = false;
        }
    }
}