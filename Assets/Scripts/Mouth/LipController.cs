using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DigitalRuby.Tween;


namespace FaceGiants {
    public class LipController : MonoBehaviour
    {
        public bool isOpened;
        public bool isStopped = true;
        public float moveDistance = 1;
        public bool isFlipped = false;

        void MoveLipTween(bool doOpen)
        {
            System.Action<ITween<Vector2>> updateLipPosition = (t) =>
            {
                transform.position = t.CurrentValue;
            };

            System.Action<ITween<Vector2>> onMoveCompletion = (t) => {
                isStopped = true;
            };

            isStopped = false;

            Vector2 targetPosition = new Vector2(
                0.0f,
                transform.position.y + (doOpen ? 1 : -1) * (isFlipped ? -1 : 1) * moveDistance
            );

            gameObject.Tween(
                "MoveLip" + (isFlipped ? "bottom" : "upper"),
                transform.position,
                targetPosition,
                1,
                TweenScaleFunctions.CubicEaseInOut,
                updateLipPosition,
                onMoveCompletion
            );
        }

        public void OpenLip()
        {
            if (!isOpened)
            {
                MoveLipTween(true);
            }
        }

        public void CloseLip()
        {
            if (isOpened)
            {
                MoveLipTween(false);
            }
        }
    }
}
