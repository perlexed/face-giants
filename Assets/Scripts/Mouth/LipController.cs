using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FaceGiants {
    public class LipController : MonoBehaviour
    {
        public bool isOpened;
        public bool doMove = false;
        private Vector2 targetPosition;

        public float speed = 1;
        public float moveDistance = 1;

        public bool isFlipped = false;
        private float moveMultiplier;

        public float debugRangeToTarget;

        private float step;

        private LipsController.OnLipStatusChange callback;

        void Start()
        {
            moveMultiplier = isFlipped ? -1 : 1;
        }

        // tween
        //DoTween
        //LeanTween

        void Update()
        {
            if (doMove)
            {
                debugRangeToTarget = Math.Abs(transform.position.y - targetPosition.y);
                Log("<b>range</b>: " + debugRangeToTarget, 1);
                if (transform.position.y != targetPosition.y)
                {
                    step = speed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                } else
                {
                    isOpened = !isOpened;
                    callback(isOpened);
                    callback = null;
                    doMove = false;
                    Log("reached target, stopping, now status is <b>" + (isOpened ? "opened" : "closed") + "</b>");
                    Debug.Break();
                }
            }
        }

        public bool IsRunning { get { return !doMove; } }

        public void OpenLip(LipsController.OnLipStatusChange funcCallback)
        {
            IsRunning = true;
            LeanTween.Move(transform, targetPosition, 2f).OnFinish(_ => IsRunning = false);

            callback = funcCallback;
            Log("<b>open</b> lip");
            //Debug.Break();

            if (!isOpened)
            {
                doMove = true;
                targetPosition = new Vector2(0.0f, transform.position.y + moveMultiplier * moveDistance);
            }
        }

        public void CloseLip(LipsController.OnLipStatusChange funcCallback)
        {
            callback = funcCallback;
            Log("<b>close</b> lip");
            //Debug.Break();

            if (isOpened)
            {
                doMove = true;
                targetPosition = new Vector2(0.0f, transform.position.y - moveMultiplier * moveDistance);
            }
        }

        private void Log(string message, int lipId = 0)
        {
            if (
                (lipId == 1 && isFlipped) || (lipId == 2 && !isFlipped)
                )
            {
                return;
            }
            string lipPrefix = "<b>" + (isFlipped ? "bottom" : "top") + "</b> lip: ";
            Debug.Log(lipPrefix + message, this);
        }
    }
}
