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
    [CreateAssetMenu(fileName = "CookingData", menuName = "B409/Jewerly Data")]
    public class JewerlyData : ScriptableObject {
        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Recipe")]
        [SerializeField]
        private List<MaterialData> materials;
        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Result")]
        [HideLabel]
        [SerializeField]
        private MaterialData result;
        [BoxGroup("Rewards")]
        [BoxGroup("Rewards/Parameter")]
        [SerializeField]
        private List<ParameterReward> parameterRewards;

        public List<MaterialData> Materials => materials;
        public MaterialData Result => result;
        public List<ParameterReward> ParameterRewards => parameterRewards;
    }
}