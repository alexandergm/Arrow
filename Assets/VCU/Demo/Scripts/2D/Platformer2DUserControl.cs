using System;
using UnityEngine;
using VirtualCameras.CrossPlatformInput;

namespace VirtualCameras.Demo.Characters
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
        }

        private void Update()
        {
            if (!jump)
            {
                jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

            // Pass all parameters to the character control script.
            character.Move(horizontal, crouch, jump);
            jump = false;
        }
    }
}
