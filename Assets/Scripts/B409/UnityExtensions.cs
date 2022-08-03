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
        private static byte ToByte(float f) {
            f = Mathf.Clamp01(f);
            return (byte)(f * 255);

        }
        public static string GetHexString(this Color color, bool includeSharp = true) {
            return string.Format("{0}{1:X2}{2:X2}{3:X2}", includeSharp ? "#" : "", ToByte(color.r), ToByte(color.g), ToByte(color.b));
        }
    }
}