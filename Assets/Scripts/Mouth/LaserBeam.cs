using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;


namespace FaceGiants
{
    public class LaserBeam : MonoBehaviour
    {
        public float LaserFireDuration = 4f;
        [Range(0f, 1f)]
        public float SemiTransparency = 0.5f;
        public float LaserTransitionTime = 1f;

        private bool _isFullPower = false;
        private SpriteRenderer _spriteRenderer;
        private Color _visibleColor;
        private Color _translucentColor;
        private Color _invisibleColor;

        private ITween<Color> _contextTween;

        private void UpdateColorLocal (ITween<Color> tween)
        {
            _spriteRenderer.color = tween.CurrentValue;
        }

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            bool isFlipped = transform.parent.GetComponent<LaserPoint>().IsUpperJaw;

            _visibleColor = _spriteRenderer.GetColorByTransparency(1f);
            _translucentColor = _spriteRenderer.GetColorByTransparency(SemiTransparency);
            _invisibleColor = _spriteRenderer.GetColorByTransparency(0f);

            _spriteRenderer.color = _translucentColor;
            _spriteRenderer.flipY = !isFlipped;
            _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, 1f);

            BoxCollider2D laserBeamCollider = gameObject.GetComponent<BoxCollider2D>();
            laserBeamCollider.size = new Vector2(_spriteRenderer.size.x, 1f);
            laserBeamCollider.offset = new Vector2(laserBeamCollider.offset.x, isFlipped ? 0.5f : -0.5f);
        }

        public void OnDestroy()
        {
            if (_contextTween != null)
            {
                _contextTween.Stop(TweenStopBehavior.DoNotModify);
            }
        }

        public IEnumerator Fire()
        {
            yield return WarmUp();
            yield return new WaitForSeconds(LaserFireDuration);
            yield return CoolDown();
            yield return new WaitForSeconds(LaserFireDuration);
            yield return TurnOff();
        }

        private IEnumerator WarmUp()
        {
            bool isTweenCompleted = false;

            System.Action<ITween<Color>> onColorChangeFinish = tween =>
            {
                isTweenCompleted = true;
                _isFullPower = true;
            };

            _contextTween = gameObject.Tween("WarmUpTween", _translucentColor, _visibleColor, LaserTransitionTime, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, onColorChangeFinish);
            yield return new WaitUntil(() => isTweenCompleted == true);
        }

        private IEnumerator CoolDown()
        {
            bool isTweenCompleted = false;
            System.Action<ITween<Color>> afterColorChange = tween => {
                isTweenCompleted = true;
            };

            _contextTween = gameObject.Tween("CoolDownTween", _visibleColor, _translucentColor, LaserTransitionTime, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, afterColorChange);
            yield return new WaitUntil(() => isTweenCompleted == true);
        }

        private IEnumerator TurnOff()
        {
            _isFullPower = false;

            bool isTweenCompleted = false;
            System.Action<ITween<Color>> afterColorChange = tween => {
                isTweenCompleted = true;
            };

            _contextTween = gameObject.Tween("TurnOffTween", _translucentColor, _invisibleColor, LaserTransitionTime, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, afterColorChange);
            yield return new WaitUntil(() => isTweenCompleted == true);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            CheckPlayerHit(collider);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            CheckPlayerHit(collider);
        }

        private void CheckPlayerHit(Collider2D collider)
        {
            if (_isFullPower && collider.gameObject == Player.Instance.gameObject)
            {
                Player.Instance.GetHitByLaser();
            }
        }
    }
}
