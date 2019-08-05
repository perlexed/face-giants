using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class EyeShooter : MonoBehaviour
    {
        public float BulletSpeed = 5f;
        public Rigidbody2D BulletPrefab;
        public GameObject BulletContainer;

        public void Shoot()
        {
            // Create bullet object
            Rigidbody2D bulletInstance = Instantiate(
                // based on bullet prefab
                BulletPrefab,
                // at the center of eye point
                transform.position,
                // with no rotation
                Quaternion.Euler(new Vector3(0, 0, 0)),
                // put it in bullet container
                BulletContainer.transform
            ) as Rigidbody2D;

            // Rotate the bullet to face the target
            Vector2 vectorToTarget = Player.Instance.transform.position - transform.position;
            bulletInstance.transform.right = vectorToTarget;

            // Normalizing direction vector to make bullet speed independent of the distance to target
            bulletInstance.velocity = vectorToTarget.normalized * BulletSpeed;
        }
    }
}
