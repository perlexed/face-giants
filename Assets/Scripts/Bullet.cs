using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class Bullet : MonoBehaviour
    {
        public float LifeTime = 2f;

        private void Start()
        {
            Destroy(gameObject, LifeTime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject == Player.Instance.gameObject) {
                Player.Instance.GetHitByBullet();
            }
        }
    }
}