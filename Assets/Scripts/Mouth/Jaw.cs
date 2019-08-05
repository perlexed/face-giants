using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

namespace FaceGiants
{
    public class Jaw : MonoBehaviour
    {
        public bool IsUpperJaw;
        public float ToothMoveDistance = 1f;
        public float ToothMoveTime = 1f;
        public Color ToothPaintColor = Color.red;

        [Space]
        public GameObject teethContainer;
        public GameObject laserPointsContainer;

        private GameObject _contextTooth;
        private LaserPoint _contextLaserPoint;
        private Color _originalTeethColor;

        private Vector2 _startToothPosition;
        private Vector2 _endToothPosition;

        public IEnumerator FireRandomTeethAndWaitForFinish()
        {
            int randomChildIndex = Random.Range(1, teethContainer.transform.childCount);
            _contextTooth = teethContainer.transform.GetChild(randomChildIndex).gameObject;
            _contextLaserPoint = laserPointsContainer.transform.GetChild(randomChildIndex).GetComponent<LaserPoint>();
            _originalTeethColor = _contextTooth.GetComponent<SpriteRenderer>().color;

            yield return TeethFireCycle();
        }

        private IEnumerator TeethFireCycle()
        {
            _startToothPosition = _contextTooth.transform.position;
            _endToothPosition = new Vector2(_startToothPosition.x, _startToothPosition.y + (IsUpperJaw ? 1 : -1) * ToothMoveDistance);

            yield return MoveToothAndWaitForFinish(true);
            yield return new WaitForSeconds(1f);
            yield return _contextLaserPoint.FireLazerCycle();
            yield return new WaitForSeconds(1f);
            yield return MoveToothAndWaitForFinish(false);
        }

        private IEnumerator MoveToothAndWaitForFinish(bool isMovingOut)
        {
            bool isMoveCompleted = false;
            Vector2 contextStartPos = isMovingOut ? _startToothPosition : _endToothPosition;
            Vector2 contextEndPos = isMovingOut ? _endToothPosition : _startToothPosition;

            System.Action<ITween<Vector2>> updateToothPos = tween =>
            {
                _contextTooth.transform.position = tween.CurrentValue;
            };

            System.Action<ITween<Vector2>> onMoveFinish = tween => {
                isMoveCompleted = true;
            };

            _contextTooth.GetComponent<SpriteRenderer>().color = isMovingOut ? ToothPaintColor : _originalTeethColor;
            _contextTooth.Tween(
                "MoveTooth",
                contextStartPos,
                contextEndPos,
                ToothMoveTime,
                TweenScaleFunctions.CubicEaseInOut,
                updateToothPos,
                onMoveFinish
            );

            yield return new WaitUntil(() => isMoveCompleted == true);
        }

        public void Reset()
        {
            _contextTooth.GetComponent<SpriteRenderer>().color = _originalTeethColor;
            _contextTooth.transform.position = _startToothPosition;
            _contextLaserPoint.Reset();
        }
    }

}
