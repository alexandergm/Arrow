using UnityEngine;
using System.Collections;

public class JOPA : MonoBehaviour
{

    public GameObject Player;
   
    public int Speed = 5;

    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            Player.transform.position += Player.transform.up * Speed * Time.deltaTime/2;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Player.transform.position -= Player.transform.up * Speed * Time.deltaTime ;
        }

        if (Input.GetKey(KeyCode.W))
        {
            Player.transform.position += Player.transform.forward * Speed * Time.deltaTime ;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Player.transform.position -= Player.transform.forward * Speed * Time.deltaTime ;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Player.transform.position -= Player.transform.right * Speed * Time.deltaTime ;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Player.transform.position += Player.transform.right * Speed * Time.deltaTime ;
        }

     
        
    }
}
