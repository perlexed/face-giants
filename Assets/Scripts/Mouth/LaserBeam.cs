using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;


namespace FaceGiants
{
    public class LaserBeam : MonoBehaviour
    {
        public float LaserFireDuration = 3f;
        public float SemiTransparency = 0.5f;

        private bool _isFullPower = false;
        private SpriteRenderer _spriteRenderer;
        private Color _visibleColor;
        private Color _translucentColor;
        private Color _invisibleColor;

        private void UpdateColorLocal (ITween<Color> tween)
        {
            _spriteRenderer.color = tween.CurrentValue;
        }

        private void Start()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            bool isFlipped = transform.parent.GetComponent<LaserPoint>().IsUpperJaw;

            _visibleColor = ColorHelper.GetColorByTransparency(_spriteRenderer, 1f);
            _translucentColor = ColorHelper.GetColorByTransparency(_spriteRenderer, SemiTransparency);
            _invisibleColor = ColorHelper.GetColorByTransparency(_spriteRenderer, 0f);

            _spriteRenderer.color = ColorHelper.GetColorByTransparency(_spriteRenderer, SemiTransparency);
            _spriteRenderer.flipY = !isFlipped;
            _spriteRenderer.size = new Vector2(_spriteRenderer.size.x, 1f);

            BoxCollider2D laserBeamCollider = gameObject.GetComponent<BoxCollider2D>();
            laserBeamCollider.size = new Vector2(_spriteRenderer.size.x, 1f);
            laserBeamCollider.offset = new Vector2(laserBeamCollider.offset.x, isFlipped ? 0.5f : -0.5f);
        }

        public void OnDestroy()
        {
            
        }

        public IEnumerator Fire()
        {
            yield return WarmUp();
            yield return new WaitForSeconds(LaserFireDuration);
            yield return TurnOff();
        }

        public IEnumerator WarmUp()
        {
            bool isTweenCompleted = false;

            System.Action<ITween<Color>> onColorChangeFinish = tween =>
            {
                isTweenCompleted = true;
                _isFullPower = true;
            };

            gameObject.Tween("FlashVisibilityTween", _translucentColor, _invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal)
                .ContinueWith(new ColorTween().Setup(_invisibleColor, _translucentColor, 1, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal))
                .ContinueWith(new ColorTween().Setup(_translucentColor, _invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal))
                .ContinueWith(new ColorTween().Setup(_invisibleColor, _visibleColor, 2, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, onColorChangeFinish));

            yield return new WaitUntil(() => isTweenCompleted == true);
        }

        public IEnumerator TurnOff()
        {
            bool isTweenCompleted = false;
            System.Action<ITween<Color>> afterColorChange = tween => {
                isTweenCompleted = true;
            };

            gameObject.Tween("FlashVisibilityTween", _visibleColor, _translucentColor, 1, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, afterColorChange);
            yield return new WaitUntil(() => isTweenCompleted == true);

            _isFullPower = false;
            isTweenCompleted = false;
            gameObject.Tween("FlashVisibilityTween", _translucentColor, _invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, UpdateColorLocal, afterColorChange);
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
                Player.Instance.GetComponent<Player>().GetHitByLaser();
            }
        }
    }
}
