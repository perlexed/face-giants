using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FaceGiants
{
    public class GameTimelineController : MonoBehaviour
    {
        public bool isRunning = false;
        private float gameStartTime = 0f;

        public Text descriptionText;
        public Text timerText;

        protected bool isGameEnded = false;

        public float eyeActionInterval = 5f;
        public GameObject eyesContainer;
        public GameObject lipsContainer;
        public GameObject mouthContainer;
        public GameObject bulletsContainer;

        public GameObject heroObject;
        protected PlayerController playerController;

        protected EyeShooter[] eyesShooters;

        protected Coroutine shootingCoroutine;
        protected Coroutine mouthCoroutine;

        void Start()
        {
            eyesShooters = eyesContainer.GetComponentsInChildren<EyeShooter>();
            playerController = heroObject.GetComponent<PlayerController>();
        }

        private void Update()
        {
            if (!isRunning && Input.GetButtonUp("Jump"))
            {
                StartGame();
            }
        }

        private void FixedUpdate()
        {
            if (isRunning)
            {
                timerText.text = (Time.realtimeSinceStartup - gameStartTime).ToString("F2");
            }
        }

        private void StartGame()
        {
            if (isRunning)
            {
                return;
            }

            Time.timeScale = 1;

            ResetGame();

            SetRunning(true);
            gameStartTime = Time.realtimeSinceStartup;
            timerText.enabled = true;
            descriptionText.enabled = false;

            shootingCoroutine = StartCoroutine(InitiateShooting());
            mouthCoroutine = StartCoroutine(mouthContainer.GetComponent<MouthController>().LifecycleCoroutine());
        }

        private void StopGame()
        {
            isGameEnded = true;
            descriptionText.text = "You died!\n\nPress space to restart";
            descriptionText.enabled = true;
            SetRunning(false);

            StopCoroutine(shootingCoroutine);
            StopCoroutine(mouthCoroutine);

            Time.timeScale = 0;
        }

        public void PlayerDead()
        {
            StopGame();
        }

        IEnumerator InitiateShooting()
        {
            bool useFirstShooter = false;
            for (; ; )
            {
                EyeShooter contextShooter = eyesShooters[useFirstShooter ? 0 : 1];
                contextShooter.Shoot();
                useFirstShooter = !useFirstShooter;

                yield return new WaitForSeconds(eyeActionInterval);
            }
        }

        public void SetRunning(bool status)
        {
            isRunning = status;

            playerController.SetMovementStatus(status);
        }

        void ResetGame()
        {
            playerController.Reset();

            foreach (Transform bulletTransform in bulletsContainer.transform)
            {
                Destroy(bulletTransform.gameObject);
            }

            mouthContainer.GetComponent<MouthController>().Reset();
        }
    }
}
