using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    public enum Parameter : int { Deft, Strength, Intelligence, Luck, Endurance };

    public static class ParameterExtensions {
        public static string ToCompatString(this Parameter parameter, bool lowerScale = true) {
            string str = "";
            switch(parameter) {
            case Parameter.Deft:
                str = "DFT";
                break;
            case Parameter.Strength:
                str = "STR";
                break;
            case Parameter.Intelligence:
                str = "INT";
                break;
            case Parameter.Luck:
                str = "LUK";
                break;
            case Parameter.Endurance:
                str = "EDR";
                break;
            default:
                break;
            }
            if(lowerScale)
                str = str.ToLower();
            return str;
        }
    }
}