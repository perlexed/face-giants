using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class EyeShooter : MonoBehaviour
    {
        public Rigidbody2D bullet;
        public GameObject target;
        public GameObject bulletContainer;

        public void Shoot()
        {
            Quaternion bulletQuaternion = Quaternion.Euler(new Vector3(0, 0, 0));
            Rigidbody2D bulletInstance = Instantiate(
                bullet,
                transform.position,
                bulletQuaternion,
                bulletContainer.transform
                ) as Rigidbody2D;
            
            bulletInstance.transform.right = target.transform.position - transform.position;

            Vector2 toHero = target.transform.position - transform.position;
            toHero.Normalize();
            
            bulletInstance.velocity = toHero * 5f;

            // Setting the target
            bulletInstance.gameObject.GetComponent<BulletController>().hero = target;
        }
    }
}
