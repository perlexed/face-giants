using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

namespace FaceGiants
{
    public class LaserPoint : MonoBehaviour
    {
        public bool IsUpperJaw;
        public float VisibilityChangeDuration = 1f;
        public GameObject LaserBeamPrefab;

        private bool _isLaserBeamCreated = false;
        private Color _visibleColor;
        private Color _invisibleColor;
        private SpriteRenderer _spriteRenderer;
        private GameObject _laserInstance;

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            _visibleColor = ColorHelper.GetColorByTransparency(_spriteRenderer.color, 1);
            _invisibleColor = ColorHelper.GetColorByTransparency(_spriteRenderer.color, 0);
        }

        public IEnumerator FireLazerCycle()
        {
            yield return ChangeVisibility(true);
            yield return FireLaser();
            yield return ChangeVisibility(false);
        }

        private IEnumerator FireLaser()
        {
            _laserInstance = Instantiate(LaserBeamPrefab, transform);
            _isLaserBeamCreated = true;

            yield return new WaitForSeconds(1);
            yield return _laserInstance.GetComponent<LaserBeam>().Fire();

            Destroy(_laserInstance);
            _isLaserBeamCreated = false;
        }

        private IEnumerator ChangeVisibility(bool makeVisible)
        {
            bool isColorChangeCompleted = false;
            Color startingColor = makeVisible ? _invisibleColor : _visibleColor;
            Color endingColor = makeVisible ? _visibleColor : _invisibleColor;

            System.Action<ITween<Color>> updateColor = tween =>
            {
                _spriteRenderer.color = tween.CurrentValue;
            };

            System.Action<ITween<Color>> onColorChangeFinish = tween =>
            {
                isColorChangeCompleted = true;
            };

            gameObject.Tween(
                "MakeVisibleTween",
                startingColor,
                endingColor,
                VisibilityChangeDuration,
                TweenScaleFunctions.CubicEaseIn,
                updateColor,
                onColorChangeFinish
            );

            yield return new WaitUntil(() => isColorChangeCompleted == true);
        }

        public void Reset()
        {
            _spriteRenderer.color = _invisibleColor;

            if (_isLaserBeamCreated)
            {
                Destroy(_laserInstance);
            }
        }
    }
}
