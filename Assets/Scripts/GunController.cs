using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants {
    public class GunController : MonoBehaviour
    {
        public Rigidbody2D bullet;
        public float gunSpeed = 0.2f;

        protected PlayerController playerController;

        protected float lastFireTime = 0f;

        void Start()
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                float currentTime = Time.time;

                if (lastFireTime == 0f || currentTime - lastFireTime > gunSpeed)
                {
                    Vector2 bulletVelocity = playerController.isFacingUp ? new Vector2(0, 10f) : new Vector2(10f, 0);
                    float bulletDirection = playerController.isFacingUp ? 90f : 0f;

                    Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, bulletDirection))) as Rigidbody2D;
                    bulletInstance.velocity = bulletVelocity;
                    lastFireTime = currentTime;
                }

            }
        }
    }
}
