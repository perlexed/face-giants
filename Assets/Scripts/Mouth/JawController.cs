using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

namespace FaceGiants
{
    public class JawController : MonoBehaviour
    {
        public bool isUpperJaw;
        public float toothMoveDistance = 1f;
        public Color toothPaintColor = Color.red;

        public GameObject teethContainer;
        public GameObject laserPointsContainer;

        private GameObject contextTooth;
        private GameObject contextLaserPoint;
        private Color originalTeethColor;

        private Vector2 startToothPosition;
        private Vector2 endPos;

        public float toothMoveTime = 1f;

        public IEnumerator FireRandomTeethAndWaitForFinish()
        {
            int randomChildIndex = Random.Range(1, teethContainer.transform.childCount);
            contextTooth = teethContainer.transform.GetChild(randomChildIndex).gameObject;
            contextLaserPoint = laserPointsContainer.transform.GetChild(randomChildIndex).gameObject;
            originalTeethColor = contextTooth.GetComponent<SpriteRenderer>().color;

            yield return TeethFireCycle();
        }

        IEnumerator TeethFireCycle()
        {
            startToothPosition = contextTooth.transform.position;
            endPos = new Vector2(startToothPosition.x, startToothPosition.y + (isUpperJaw ? 1 : -1) * toothMoveDistance);

            yield return MoveToothAndWaitForFinish(true);
            yield return new WaitForSeconds(1);
            yield return contextLaserPoint.GetComponent<LaserPointController>().FireLazerCycle();
            yield return new WaitForSeconds(1);
            yield return MoveToothAndWaitForFinish(false);
        }

        // @todo get rid of WaitForSeconds, yield return on Tween callback
        IEnumerator MoveToothAndWaitForFinish(bool isMovingOut)
        {
            Vector2 contextStartPos = isMovingOut ? startToothPosition : endPos;
            Vector2 contextEndPos = isMovingOut ? endPos : startToothPosition;

            System.Action<ITween<Vector2>> updateToothPos = (t) =>
            {
                contextTooth.transform.position = t.CurrentValue;
            };

            contextTooth.Tween("MoveTooth", contextStartPos, contextEndPos, toothMoveTime, TweenScaleFunctions.CubicEaseInOut, updateToothPos);

            contextTooth.GetComponent<SpriteRenderer>().color = isMovingOut ? toothPaintColor : originalTeethColor;

            yield return new WaitForSeconds(toothMoveTime);
        }

        public void Reset()
        {
            contextTooth.GetComponent<SpriteRenderer>().color = originalTeethColor;
            contextTooth.transform.position = startToothPosition;
            contextLaserPoint.GetComponent<LaserPointController>().Reset();

            // @todo refactor this
            StopAllCoroutines();
        }
    }

}
