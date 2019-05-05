using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants {
    public class BulletController : MonoBehaviour
    {
        public GameObject hero;

        protected PlayerController playerController;

        void Start()
        {
            Destroy(gameObject, 2);
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            GameObject colliderGameObject = col.gameObject;

            if (col.gameObject == hero)
            {
                //Debug.Log("hero");
            } else
            {
                //Debug.Log("no hero");
            }
        }
    }
}