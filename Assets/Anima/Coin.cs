using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
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
            Destroy(obj);
        }
    }
}