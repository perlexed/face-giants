using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants {
    public class BulletController : MonoBehaviour
    {
        public GameObject hero;
        public bool isHeroGunBullet = false;

        void Start()
        {
            Destroy(gameObject, 2);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!isHeroGunBullet && collider.gameObject == hero) {
                hero.GetComponent<PlayerController>().GetHitByBullet();
            }
        }
    }
}