using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DigitalRuby.Tween;


namespace FaceGiants
{
    public class Lip : MonoBehaviour
    {
        public bool IsStopped = true;
        public bool IsUpperLip = false;
        public float LipOpenDuration = 1f;

        private Vector2 _defaultPosition;
        private float _moveDistance = 1f;

        private void Start()
        {
            _defaultPosition = transform.localPosition;
        }

        public void Reset()
        {
            transform.localPosition = _defaultPosition;
        }

        public void OpenLip()
        {
            MoveLipTween(true);
        }

        public void CloseLip()
        {
            MoveLipTween(false);
        }

        private void MoveLipTween(bool doOpen)
        {
            IsStopped = false;
            float targetYPosition = transform.position.y + (doOpen ? 1 : -1) * (IsUpperLip ? 1 : -1) * _moveDistance;

            // Move lip vertically from current position
            gameObject.Tween(
                IsUpperLip ? "MoveUpperLipTween" : "MoveBottomLipTween",
                transform.position,
                new Vector2(0, targetYPosition),
                LipOpenDuration,
                TweenScaleFunctions.CubicEaseInOut,
                UpdateLipPosition,
                OnMoveCompletion
            );
        }

        private void UpdateLipPosition(ITween<Vector2> tween)
        {
            transform.position = tween.CurrentValue;
        }

        private void OnMoveCompletion(ITween<Vector2> tween)
        {
            IsStopped = true;
        }
    }
}
