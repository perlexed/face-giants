using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants {
    public class PlayerController : MonoBehaviour
    {

        public float speed = 50f;
        public float maxSpeed = 300f;
        public float jumpForce = 250f;
        public float velocitySign;
        public float limitedVelocity;
        public float dCurSpeed;
        public float dMaxSpeed;

        public SpriteRenderer heroUpSprite;
        public SpriteRenderer heroNormalSprite;

        public Transform groundcheck;

        [HideInInspector]
        public bool isFacingUp = false;

        protected bool isFacingRight = true;
        protected bool isGrounded;
        protected bool doJump = false;

        private void Update()
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            isGrounded = Physics2D.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("ground"));

            // If the jump button is pressed and the player is grounded then the player should jump.
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                doJump = true;
            }
        }

        void FixedUpdate()
        {
            float h = Input.GetAxis("Horizontal");

            GetComponent<Rigidbody2D>().AddForce(Vector2.right * h * speed);

            float upAxisValue = Input.GetAxis("Vertical");

            isFacingUp = upAxisValue > 0;

            heroNormalSprite.enabled = !isFacingUp;
            heroUpSprite.enabled = isFacingUp;

            if (h < 0)
            {
                //GetComponent<SpriteRenderer>().flipX = true;
            }

            if (doJump)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
                doJump = false;
            }

            // If the player's horizontal velocity is greater than the maxSpeed...
            dCurSpeed = GetComponent<Rigidbody2D>().velocity.x;
            dMaxSpeed = Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x);
            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
            {
                // ... set the player's velocity to the maxSpeed in the x axis.
                velocitySign = Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x);
                limitedVelocity = velocitySign * maxSpeed;
                GetComponent<Rigidbody2D>().velocity = new Vector2(limitedVelocity, GetComponent<Rigidbody2D>().velocity.y);
            }

        }
    }
}
