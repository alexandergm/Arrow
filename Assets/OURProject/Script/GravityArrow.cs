using UnityEngine;
using System.Collections;

public class GravityArrow : MonoBehaviour
{

    public GameObject obj;
    public ParticleSystem particleLauncher;
    public ParticleSystem particleLauncher2;
    public ParticleSystem particleLauncher3;

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
            particleLauncher.Emit(1);
            particleLauncher2.Emit(1);
            particleLauncher3.Emit(1);

        }
    }
}