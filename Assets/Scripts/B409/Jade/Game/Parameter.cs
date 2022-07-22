using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    public enum Parameter : int { Deft, Strength, Intelligence, Luck, Endurance };

    public static class ParameterExtensions {
        public static string ToCompatString(this Parameter parameter) {
            switch(parameter) {
            case Parameter.Deft:
                return "DFT";
            case Parameter.Strength:
                return "STR";
            case Parameter.Intelligence:
                return "INT";
            case Parameter.Luck:
                return "LUK";
            case Parameter.Endurance:
                return "EDR";
            default:
                return "";
            }
        }
    }
}