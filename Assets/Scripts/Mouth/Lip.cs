using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DigitalRuby.Tween;


namespace FaceGiants
{
    public class Lip : MonoBehaviour
    {
        public bool IsUpperLip = false;
        public float LipOpenDuration = 1f;
        public float _moveDistance = 1f;

        public bool IsStopped { get; private set; } = true;

        private Vector2 _defaultPosition;
        private ITween<Vector2> _moveTween;

        private void Start()
        {
            _defaultPosition = transform.localPosition;
        }

        public void Reset()
        {
            transform.localPosition = _defaultPosition;

            if (_moveTween != null)
            {
                _moveTween.Stop(TweenStopBehavior.Complete);
            }
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

            Action<ITween<Vector2>> updateLipPosition = tween =>
            {
                transform.position = tween.CurrentValue;
            };

            Action<ITween<Vector2>> onMoveCompletion = tween => {
                IsStopped = true;
            };

            // Move lip vertically from current position
            _moveTween = gameObject.Tween(
                IsUpperLip ? "MoveUpperLipTween" : "MoveBottomLipTween",
                transform.position,
                new Vector2(0, targetYPosition),
                LipOpenDuration,
                TweenScaleFunctions.CubicEaseInOut,
                updateLipPosition,
                onMoveCompletion
            );
        }
    }
}
