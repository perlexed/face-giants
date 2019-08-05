using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FaceGiants
{
    class ColorHelper
    {
        public static Color GetColorByTransparency(Color color, float transparency)
        {
            return new Color(color.r, color.g, color.b, transparency);
        }

        public static Color GetColorByTransparency(SpriteRenderer spriteRenderer, float transparency)
        {
            return GetColorByTransparency(spriteRenderer.color, transparency);
        }
    }
}
