using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualCameras;

namespace VirtualCameras.Demo
{
    public class DemoManager : MonoBehaviour
    {
        public Characters.ThirdPersonUserControl userControl3DPlayer1;
        public Characters.Platformer2DUserControl userControl2DPlayer1;
        public Characters.Platformer2DUserControl userControl2DPlayer2;
        public List<GameObject> cameras;
        public string defaultCamera = "RPGCamera";

        // Use this for initialization
        void Start()
        {
            foreach(GameObject cam in cameras)
            {
                if(cam.name == defaultCamera)
                {
                    cam.SetActive(true);
                }
                else
                {
                    cam.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Change the currently active camera based on the parameter
        public void ChangeCamera(string name)
        {
            if(name != "RPGCamera" && name != "SmoothFollowCamera" && name != "SmoothFollow2DCamera" && name != "MultiTarget2DCamera")
            {
                if(SceneManager.GetActiveScene().name == "2D")
                {
                    userControl2DPlayer1.gameObject.SetActive(false);
                    userControl2DPlayer2.gameObject.SetActive(false);
                }
                else
                {
                    userControl3DPlayer1.gameObject.SetActive(false);
                }
            }
            else
            {
                if(SceneManager.GetActiveScene().name == "2D")
                {
                    userControl2DPlayer1.gameObject.SetActive(true);
                    userControl2DPlayer2.gameObject.SetActive(true);
                }
                else
                {
                    userControl3DPlayer1.gameObject.SetActive(true);
                }
            }

            foreach(GameObject cam in cameras)
            {
                if(cam.name == name)
                {
                    cam.SetActive(true);
                }
                else
                {
                    cam.SetActive(false);
                }
            }
        }
    }
}
