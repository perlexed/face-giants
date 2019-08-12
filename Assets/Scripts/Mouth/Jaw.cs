using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;
using System;
using Random = UnityEngine.Random;

namespace FaceGiants
{
    public class Jaw : MonoBehaviour
    {
        public bool IsUpperJaw;
        public float ToothMoveDistance = 1f;
        public float ToothMoveTime = 1f;
        public float PauseTime = 1f;
        public Color ToothPaintColor = Color.red;

        [Space]
        public SpriteRenderer[] Teeth;
        public LaserPoint[] LaserPoints;

        private SpriteRenderer _contextToothSpriteRenderer;
        private LaserPoint _contextLaserPoint;
        private Color _originalTeethColor;

        private Vector2 _startToothPosition;
        private Vector2 _endToothPosition;

        private ITween<Vector2> _toothMoveTween;

        public IEnumerator FireRandomTeethAndWaitForFinish()
        {
            int randomChildIndex = Random.Range(1, Teeth.Length);
            _contextToothSpriteRenderer = Teeth[randomChildIndex];
            _contextLaserPoint = LaserPoints[randomChildIndex];
            _originalTeethColor = _contextToothSpriteRenderer.color;

            yield return TeethFireProcedure();
        }

        private IEnumerator TeethFireProcedure()
        {
            _startToothPosition = _contextToothSpriteRenderer.transform.position;
            _endToothPosition = new Vector2(_startToothPosition.x, _startToothPosition.y + (IsUpperJaw ? 1 : -1) * ToothMoveDistance);

            yield return MoveToothAndWaitForFinish(true);
            yield return new WaitForSeconds(PauseTime);
            yield return _contextLaserPoint.FireLazerCycle();
            yield return new WaitForSeconds(PauseTime);
            yield return MoveToothAndWaitForFinish(false);
        }

        private IEnumerator MoveToothAndWaitForFinish(bool isMovingOut)
        {
            bool isMoveCompleted = false;
            Vector2 contextStartPos = isMovingOut ? _startToothPosition : _endToothPosition;
            Vector2 contextEndPos = isMovingOut ? _endToothPosition : _startToothPosition;

            Action<ITween<Vector2>> updateToothPosition = tween =>
            {
                _contextToothSpriteRenderer.transform.position = tween.CurrentValue;
            };

            Action<ITween<Vector2>> onMoveFinish = tween => {
                isMoveCompleted = true;
            };

            _contextToothSpriteRenderer.color = isMovingOut ? ToothPaintColor : _originalTeethColor;
            _toothMoveTween = _contextToothSpriteRenderer.gameObject.Tween(
                "MoveTooth",
                contextStartPos,
                contextEndPos,
                ToothMoveTime,
                TweenScaleFunctions.CubicEaseInOut,
                updateToothPosition,
                onMoveFinish
            );

            yield return new WaitUntil(() => isMoveCompleted == true);
        }

        public void Reset()
        {
            _contextToothSpriteRenderer.color = _originalTeethColor;
            _contextToothSpriteRenderer.transform.position = _startToothPosition;
            _contextLaserPoint.Reset();

            if (_toothMoveTween != null)
            {
                _toothMoveTween.Stop(TweenStopBehavior.Complete);
            }
        }
    }

}
