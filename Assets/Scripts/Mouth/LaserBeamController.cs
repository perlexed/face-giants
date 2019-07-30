using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;


namespace FaceGiants
{
    public class LaserBeamController : MonoBehaviour
    {
        public static float midTransparency = 0.5f;
        public GameObject hero;
        public bool isFullPower = false;

        protected SpriteRenderer spriteRenderer;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public IEnumerator Fire()
        {
            yield return WarmUp();

            isFullPower = true;

            yield return new WaitForSeconds(3);

            yield return TurnOff();
        }

        public IEnumerator WarmUp()
        {
            Color visibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 1f);
            Color translucentColor = ColorHelper.getColorByTransparency(spriteRenderer.color, midTransparency);
            Color invisibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 0f);

            System.Action<ITween<Color>> updateColor = (t) =>
            {
                spriteRenderer.color = t.CurrentValue;
            };

            gameObject.Tween("FlashVisibilityTween", translucentColor, invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor)
                .ContinueWith(new ColorTween().Setup(invisibleColor, translucentColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor))
                .ContinueWith(new ColorTween().Setup(translucentColor, invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor))
                .ContinueWith(new ColorTween().Setup(invisibleColor, visibleColor, 2, TweenScaleFunctions.CubicEaseIn, updateColor));

            yield return new WaitUntil(() => spriteRenderer.color.a == 1f);
        }

        public IEnumerator TurnOff()
        {
            // @todo refactor duplicate code

            Color visibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 1f);
            Color translucentColor = ColorHelper.getColorByTransparency(spriteRenderer.color, midTransparency);
            Color invisibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 0f);

            System.Action<ITween<Color>> updateColor = (t) =>
            {
                spriteRenderer.color = t.CurrentValue;
            };

            gameObject.Tween("FlashVisibilityTween", visibleColor, translucentColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor);
            yield return new WaitUntil(() => spriteRenderer.color == translucentColor);

            isFullPower = false;

            gameObject.Tween("FlashVisibilityTween", translucentColor, invisibleColor, 0, TweenScaleFunctions.CubicEaseIn, updateColor);
            yield return new WaitUntil(() => spriteRenderer.color == invisibleColor);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (isFullPower && collider.gameObject == hero)
            {
                hero.GetComponent<PlayerController>().GetHitByLaser();
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (isFullPower && collider.gameObject == hero)
            {
                hero.GetComponent<PlayerController>().GetHitByLaser();
            }
        }
    }
}
