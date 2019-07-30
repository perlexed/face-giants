using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public int hitpoints;
        public int maxHitpoints = 3;

        public bool allowMovement;

        public SpriteRenderer heroUpSprite;
        public SpriteRenderer heroNormalSprite;

        public Text hitpointsText;

        public GameObject baseControllerObject;
        protected GameTimelineController gameTimelineController;

        public Transform groundcheck;

        protected Rigidbody2D playerRigidbody;

        [HideInInspector]
        public bool isFacingUp = false;

        protected bool isFacingRight = true;
        protected bool isGrounded;
        protected bool doJump = false;

        public float laserInvulterabilityTime = 1f;
        protected float lastLaserHitTime;

        private void Start()
        {
            gameTimelineController = baseControllerObject.GetComponent<GameTimelineController>();
            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetMovementStatus(bool status)
        {
            allowMovement = status;
        }

        public void Reset()
        {
            SetHitpoints(maxHitpoints);

            // Reset position
            transform.position = new Vector2(0, 0);
            playerRigidbody.velocity = new Vector2(0, 0);
        }

        private void Update()
        {
            if (allowMovement)
            {
                JumpCheck();
            }
        }

        protected void JumpCheck()
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            isGrounded = Physics2D.Linecast(transform.position, groundcheck.position, 1 << LayerMask.NameToLayer("ground"));

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                doJump = true;
            }
        }

        void FixedUpdate()
        {
            if (allowMovement)
            {
                HorizontalMovement();
            }
        }

        void HorizontalMovement()
        {
            float h = Input.GetAxis("Horizontal");

            playerRigidbody.AddForce(Vector2.right * h * speed);

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
                playerRigidbody.AddForce(new Vector2(0f, jumpForce));
                doJump = false;
            }

            // If the player's horizontal velocity is greater than the maxSpeed...
            dCurSpeed = playerRigidbody.velocity.x;
            dMaxSpeed = Mathf.Abs(playerRigidbody.velocity.x);
            if (Mathf.Abs(playerRigidbody.velocity.x) > maxSpeed)
            {
                // ... set the player's velocity to the maxSpeed in the x axis.
                velocitySign = Mathf.Sign(playerRigidbody.velocity.x);
                limitedVelocity = velocitySign * maxSpeed;
                playerRigidbody.velocity = new Vector2(limitedVelocity, playerRigidbody.velocity.y);
            }
        }

        public void GetHit()
        {
            SetHitpoints(hitpoints - 1);

            if (hitpoints == 0)
            {
                gameTimelineController.PlayerDead();
            }
        }

        public void GetHitByLaser()
        {
            if (Time.time - lastLaserHitTime < laserInvulterabilityTime)
            {
                return;
            }

            GetHit();
            lastLaserHitTime = Time.time;
        }

        public void GetHitByBullet()
        {
            GetHit();
        }

        void SetHitpoints(int hitpointsNewValue)
        {
            hitpoints = hitpointsNewValue;
            hitpointsText.text = "Hitpoints: " + hitpointsNewValue.ToString();
        }
    }
}
