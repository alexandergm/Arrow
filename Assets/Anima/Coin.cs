using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{

    public GameObject obj;
    public ParticleSystem particleLauncher;
    public ParticleSystem particleLauncher1;
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
            particleLauncher.Emit(1);
            particleLauncher1.Emit(1);
            particleLauncher2.Emit(1);
            particleLauncher3.Emit(1);
            Destroy(obj);
        }
    }
}