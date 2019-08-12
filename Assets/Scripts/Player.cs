using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FaceGiants
{
    public class Player : MonoBehaviour
    {
        [Tooltip("Laser Invulnerability Duration")]
        public float LaserInvulterabilityDuration;
        public float HorizontalAcceleration;
        public float MaxSpeed;
        public float JumpForce;

        [Range(1, 30)]
        public int MaxHitpoints;

        [Space]
        public Text HitpointsText;
        public Transform Groundcheck;

        [HideInInspector]
        public bool AllowMovement;

        private int _hitpoints;
        private bool _doJump = false;
        private float _lastLaserHitTime;
        private Rigidbody2D _playerRigidbody;
        private Vector2 _startingPosition;

        public static Player Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody2D>();
            SetHitpoints(MaxHitpoints);
            _startingPosition = transform.position;
        }

        public void Reset()
        {
            SetHitpoints(MaxHitpoints);

            transform.position = _startingPosition;
            _playerRigidbody.velocity = new Vector2(0, 0);
        }

        private void Update()
        {
            if (AllowMovement)
            {
                JumpCheck();
            }
        }

        private void JumpCheck()
        {
            // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
            bool isGrounded = Physics2D.Linecast(transform.position, Groundcheck.position, LayerMask.GetMask("ground"));

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                _doJump = true;
            }
        }

        private void FixedUpdate()
        {
            if (AllowMovement)
            {
                ProcessMovement();
            }
        }

        private void ProcessMovement()
        {
            Vector2 forcesToAdd = new Vector2(
                Input.GetAxis("Horizontal") * HorizontalAcceleration,
                _doJump ? JumpForce : 0f
            );

            _playerRigidbody.AddForce(forcesToAdd);
            _doJump = false;

            _playerRigidbody.velocity = new Vector2(
                Mathf.Clamp(_playerRigidbody.velocity.x, -MaxSpeed, MaxSpeed),
                _playerRigidbody.velocity.y
            );
        }

        public void GetHit()
        {
            SetHitpoints(_hitpoints - 1);

            if (_hitpoints == 0)
            {
                GameTimeline.Instance.PlayerDead();
            }
        }

        public void GetHitByLaser()
        {
            // Don't trigger laser hit more often that LaserInvulterabilityDuration time
            if (Time.time - _lastLaserHitTime < LaserInvulterabilityDuration)
            {
                return;
            }

            GetHit();
            _lastLaserHitTime = Time.time;
        }

        public void GetHitByBullet()
        {
            GetHit();
        }

        private void SetHitpoints(int hitpointsNewValue)
        {
            _hitpoints = hitpointsNewValue;
            HitpointsText.text = "Hitpoints: " + hitpointsNewValue.ToString();
        }
    }
}
