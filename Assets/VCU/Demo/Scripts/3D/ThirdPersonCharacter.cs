using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualCameras.CrossPlatformInput;

namespace VirtualCameras.Demo.Characters
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
        public float groundCheckDistance = 0.1f;
        public float movingTurnSpeed = 360;
        public float stationaryTurnSpeed = 180;
        public float animSpeedMultiplier = 1f;
        public float movementSpeed = 5.0f;
        public float currentMovementSpeed;
        public float maxMovementSpeed = 10f;
        public Vector3 moveDirection;

		private Rigidbody rbody;
        private Animator animator;
        private Vector3 cameraForward;
        Vector3 groundNormal;
        private const float half = 0.5f;
        private float turnAmount;
		void Start()
		{
			animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody>();

            rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            rbody.AddForce(moveDirection * currentMovementSpeed);
            animator.SetFloat("movementSpeed", currentMovementSpeed, 0.1f, Time.deltaTime);
        }

        public void Move(Vector3 move)
        {
            if(move.magnitude > 1f)
            {
                move.Normalize();
            }

            moveDirection = move;
            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, groundNormal);
            turnAmount = Mathf.Atan2(move.x, move.z);
            currentMovementSpeed = move.z;
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, currentMovementSpeed);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
            currentMovementSpeed = currentMovementSpeed * movementSpeed * Time.deltaTime;
            currentMovementSpeed = Mathf.Clamp(currentMovementSpeed, 0, maxMovementSpeed);
        }

        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
            {
                groundNormal = hitInfo.normal;
            }
            else
            {
                groundNormal = Vector3.up;
            }
        }
	}
}
