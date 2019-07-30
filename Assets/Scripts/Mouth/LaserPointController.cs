using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Tween;

namespace FaceGiants
{
    public class LaserPointController : MonoBehaviour
    {
        public GameObject laserPrefab;
        public GameObject hero;
        public bool isBottomJaw = false;

        private SpriteRenderer spriteRenderer;
        private Color visibleColor;
        private Color invisibleColor;

        protected GameObject laserInstance;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

            visibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 1);
            invisibleColor = ColorHelper.getColorByTransparency(spriteRenderer.color, 0);
        }

        public IEnumerator FireLazerCycle()
        {
            yield return MakeVisible();
            yield return FireLaser();
            yield return MakeInvisible();
        }

        public IEnumerator MakeVisible()
        {
            System.Action<ITween<Color>> updateColor = (t) =>
            {
                spriteRenderer.color = t.CurrentValue;
            };

            gameObject.Tween("MakeVisibleTween", invisibleColor, visibleColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor);
            yield return new WaitUntil(() => spriteRenderer.color.a == 1);
        }

        public IEnumerator FireLaser()
        {
            GameObject laserInstance = CreateLaserBeam();

            yield return new WaitForSeconds(2);
            yield return laserInstance.GetComponent<LaserBeamController>().Fire();
            Destroy(laserInstance);
        }

        public IEnumerator MakeInvisible()
        {
            System.Action<ITween<Color>> updateColor = (t) =>
            {
                spriteRenderer.color = t.CurrentValue;
            };

            gameObject.Tween("MakeVisibleTween", visibleColor, invisibleColor, 1, TweenScaleFunctions.CubicEaseIn, updateColor);
            yield return new WaitUntil(() => spriteRenderer.color.a == 0);
        }

        public void Reset()
        {
            spriteRenderer.color = invisibleColor;

            // @todo refactor this
            StopAllCoroutines();
            StopCoroutine("FireLaser");

            if (laserInstance)
            {
                Destroy(laserInstance);
            }
        }

        GameObject CreateLaserBeam()
        {
            laserInstance = Instantiate(laserPrefab, transform);

            SpriteRenderer laserSpriteRenderer = laserInstance.GetComponent<SpriteRenderer>();
            laserSpriteRenderer.color = ColorHelper.getColorByTransparency(laserSpriteRenderer.color, LaserBeamController.midTransparency);
            laserSpriteRenderer.flipY = isBottomJaw;
            laserSpriteRenderer.size = new Vector2(laserSpriteRenderer.size.x, 1f);

            BoxCollider2D laserBeamCollider = laserInstance.GetComponent<BoxCollider2D>();
            laserBeamCollider.size = new Vector2(laserSpriteRenderer.size.x, 1f);
            laserBeamCollider.offset = new Vector2(laserBeamCollider.offset.x, isBottomJaw ? -0.5f : 0.5f);

            laserInstance.GetComponent<LaserBeamController>().hero = hero;

            return laserInstance;
        }
    }
}
