using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utils
{
    internal static class MathUtils
    {
        public static Vector2 RotateVector(Vector2 vector, float radians)
        {
            var newX = vector.x * Mathf.Cos(radians) - vector.y * Mathf.Sin(radians);
            var newY = vector.x * Mathf.Sin(radians) + vector.y * Mathf.Cos(radians);
            return new Vector2(newX, newY);
        }
    }
}
