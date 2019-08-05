using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class Bullet : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 2);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject == Player.Instance.gameObject) {
                Player.Instance.GetHitByBullet();
            }
        }
    }
}