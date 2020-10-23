using System;
using UnityEngine;
using VirtualCameras.CrossPlatformInput;

namespace VirtualCameras.Demo.Characters
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter character;
        private Transform characterCamera;
        private Vector3 cameraForward;
        private Vector3 movement;

        private void Start()
        {
            if(Camera.main != null)
            {
                characterCamera = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("No main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            }

            character = GetComponent<ThirdPersonCharacter>();
        }

        private void Update()
        {

        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

#if !MOBILE_INPUT
            if(Input.GetKey(KeyCode.LeftShift)) movement *= 0.5f;
#endif

            if(characterCamera != null)
            {
                Vector3 forward = characterCamera.transform.TransformDirection(Vector3.forward);
                forward.y = 0;
                forward = forward.normalized;
                Vector3 right = new Vector3(forward.z, 0, -forward.x);
                movement = (horizontal * right + vertical * forward);
                movement = Quaternion.Inverse(transform.rotation) * movement;
                movement = new Vector3(movement.x, 0, movement.z);
                movement = transform.rotation * movement;
            }
            else
            {
                movement = vertical * Vector3.forward + horizontal * Vector3.right;
            }

            character.Move(movement);
        }
    }
}
