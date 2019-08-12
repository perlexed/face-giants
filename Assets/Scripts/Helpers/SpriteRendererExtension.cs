using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FaceGiants
{
    public static class SpriteRendererExtension
    {
        public static Color GetColorByTransparency(this SpriteRenderer spriteRenderer, float transparency)
        {
            return GetColorByTransparency(spriteRenderer.color, transparency);
        }

        private static Color GetColorByTransparency(Color color, float transparency)
        {
            return new Color(color.r, color.g, color.b, transparency);
        }
    }
}
