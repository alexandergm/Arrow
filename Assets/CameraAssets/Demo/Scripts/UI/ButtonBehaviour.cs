using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
	// Use this for initialization
	private void Start ()
    {
		
	}
	
	// Update is called once per frame
	private void Update ()
    {
		
	}

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void SwitchToScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
