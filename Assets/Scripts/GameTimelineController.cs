using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public class GameTimelineController : MonoBehaviour
    {

        public float eyeActionInterval = 5f;
        public GameObject eyesContainer;
        public GameObject lipsContainer;
        public GameObject mouthContainer;

        protected EyeShooter[] eyesShooters;

        void Start()
        {
            eyesShooters = eyesContainer.GetComponentsInChildren<EyeShooter>();
            StartCoroutine(InitiateShooting());

            StartCoroutine(mouthContainer.GetComponent<MouthController>().LifecycleCoroutine());
            //StartCoroutine(InitiateLipsOpenCycle());
        }

        IEnumerator InitiateShooting()
        {
            for (; ; )
            {
                foreach (EyeShooter eyeShooter in eyesShooters)
                {
                    eyeShooter.Shoot();
                }

                yield return new WaitForSeconds(eyeActionInterval);
            }
        }

        //IEnumerator InitiateLipsOpenCycle()
        //{
        //    for (; ; )
        //    {
        //        mouthContainer.GetComponent<MouthController>().RunMouthEvent();

        //        yield return new WaitForSeconds(30);
        //    }
        //}
    }
}
