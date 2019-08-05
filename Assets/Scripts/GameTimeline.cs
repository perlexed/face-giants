using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FaceGiants
{
    public class GameTimeline : MonoBehaviour
    {
        public float EyeShootingInterval = 5f;

        [TextArea(1, 3)]
        public string PlayerDiedDescription = "You died!\n\nPress space to restart";

        [Space]
        public Text DescriptionText;
        public Text TimerText;
        public EyeShooter LeftEyePoint;
        public EyeShooter RightEyePoint;
        public Mouth Mouth;
        public GameObject BulletsContainer;

        private float _gameStartTime;
        private bool _isRunning = false;

        private Coroutine _shootingCoroutine;
        private Coroutine _mouthCoroutine;

        public static GameTimeline Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void PlayerDead()
        {
            StopGame();
        }

        private void Update()
        {
            if (!_isRunning && Input.GetButtonUp("Jump"))
            {
                StartGame();
            }
        }

        private void FixedUpdate()
        {
            if (_isRunning)
            {
                TimerText.text = (Time.realtimeSinceStartup - _gameStartTime).ToString("F2");
            }
        }

        private void StartGame()
        {
            if (_isRunning)
            {
                return;
            }

            Time.timeScale = 1;

            ResetGame();

            SetRunning(true);
            _gameStartTime = Time.realtimeSinceStartup;
            TimerText.enabled = true;
            DescriptionText.enabled = false;

            _shootingCoroutine = StartCoroutine(RunEyeShooting());
            _mouthCoroutine = StartCoroutine(Mouth.RunLifecycle());
        }

        private void StopGame()
        {
            DescriptionText.text = PlayerDiedDescription;
            DescriptionText.enabled = true;
            SetRunning(false);

            StopCoroutine(_shootingCoroutine);
            StopCoroutine(_mouthCoroutine);

            Time.timeScale = 0;
        }

        private IEnumerator RunEyeShooting()
        {
            bool useLeftEye = false;
            for (; ; )
            {
                if (useLeftEye)
                {
                    LeftEyePoint.Shoot();
                } else
                {
                    RightEyePoint.Shoot();
                }

                useLeftEye = !useLeftEye;

                yield return new WaitForSeconds(EyeShootingInterval);
            }
        }

        private void SetRunning(bool status)
        {
            _isRunning = status;

            Player.Instance.AllowMovement = status;
        }

        private void ResetGame()
        {
            Player.Instance.Reset();

            foreach (Transform bulletTransform in BulletsContainer.transform)
            {
                Destroy(bulletTransform.gameObject);
            }

            Mouth.Reset();
        }
    }
}
