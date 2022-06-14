using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    using Game;

    [System.Serializable]
    public class ParameterValue {
        [SerializeField]
        [HorizontalGroup("group")]
        [HideLabel]
        private Parameter parameter;
        [SerializeField]
        [HorizontalGroup("group")]
        [HideLabel]
        private float value;

        public Parameter Parameter => parameter;
        public float Value => value;
    }
}