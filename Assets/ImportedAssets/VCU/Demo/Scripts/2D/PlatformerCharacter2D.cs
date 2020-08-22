using System;
using UnityEngine;

namespace VirtualCameras.Demo.Characters
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        public float maxMovementSpeed = 10f;
        public float jumpForce = 400f;
        [Range(0, 1)] public float crouchSpeed = 0.36f;
        public bool airControl = false;
        public LayerMask groundMask;

        private Transform groundCheck;
        private const float groundedRadius = .2f;
        private bool grounded;
        private Transform ceilingCheck;
        private const float ceilingRadius = .01f;
        private Animator animator;
        private Rigidbody2D rbody2d;
        private bool facingRight = true;

        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            animator = GetComponent<Animator>();
            rbody2d = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundMask);
            for (int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].gameObject != gameObject)
                {
                    grounded = true;
                }
            }
            animator.SetBool("Ground", grounded);

            // Set the vertical animation
            animator.SetFloat("vSpeed", rbody2d.velocity.y);
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && animator.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundMask))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            animator.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move* crouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                animator.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                rbody2d.velocity = new Vector2(move * maxMovementSpeed, rbody2d.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (grounded && jump && animator.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                grounded = false;
                animator.SetBool("Ground", false);
                rbody2d.AddForce(new Vector2(0f, jumpForce));
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
