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
        public static Color getColorByTransparency(Color color, float transparency)
        {
            return new Color(color.r, color.g, color.b, transparency);
        }
    }
}
