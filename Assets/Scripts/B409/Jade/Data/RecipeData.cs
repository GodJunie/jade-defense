// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// UnityEngine
using UnityEngine;
// OdinInspector
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    public abstract class RecipeData : ScriptableObject {
        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Recipe")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<MaterialData> materials;
        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Result")]
        [HideLabel]
        [SerializeField]
        private MaterialData result;
        [BoxGroup("Rewards")]
        [BoxGroup("Rewards/Parameter")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ParameterValue> parameterRewards;

        public List<MaterialData> Materials => materials;
        public MaterialData Result => result;
        public List<ParameterValue> ParameterRewards => parameterRewards;
    }
}