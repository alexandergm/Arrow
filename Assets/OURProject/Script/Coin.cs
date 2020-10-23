using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour
{

    public GameObject obj;
    //public GameObject starLeft;
    //public GameObject text;
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
            //text.GetComponent<Text>().text = "100";
            particleLauncher.Emit(1);
            particleLauncher1.Emit(1);
            particleLauncher2.Emit(1);
            particleLauncher3.Emit(1);
            //starLeft.SetActive(true);
            
            Destroy(obj);
        }
    }

}