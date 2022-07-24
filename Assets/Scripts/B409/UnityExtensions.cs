using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409 {
    public static class UnityExtensions {
        public static Color Lerp(this Color origin, Color color, float t) {
            return new Color(
                Mathf.Lerp(origin.r, color.r, t),
                Mathf.Lerp(origin.g, color.g, t),
                Mathf.Lerp(origin.b, color.b, t),
                Mathf.Lerp(origin.a, color.a, t)
            );
        }
    }
}