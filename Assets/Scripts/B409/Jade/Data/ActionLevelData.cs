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
    using Game;

    [Serializable]
    public abstract class ActionLevelData {
        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Action Info")]
        [HorizontalGroup("@Summary/Action Info/group", .4f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ParameterValue> inquiredParameters = new List<ParameterValue>();

        [HorizontalGroup("@Summary/Action Info/group", .4f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ParameterValue> rewardParameters = new List<ParameterValue>();

        [HorizontalGroup("@Summary/Action Info/group")]
        [BoxGroup("@Summary/Action Info/group/AP")]
        [HideLabel]
        [SerializeField]
        private float ap;

        public List<ParameterValue> InquiredParameters => inquiredParameters;
        public List<ParameterValue> RewardParameters => rewardParameters;
        public float AP => ap;

#if UNITY_EDITOR
        protected virtual string Summary {
            get {
                string summary = "";
                summary += "Inquired: ";
                foreach(var parameterValue in inquiredParameters) {
                    summary += string.Format("{0} {1}/", parameterValue.Parameter.ToCompatString(false), parameterValue.Value);
                }
                summary += "Rewards: ";
                foreach(var parameterValue in rewardParameters) {
                    summary += string.Format("{0} {1}/", parameterValue.Parameter.ToCompatString(false), parameterValue.Value);
                }
                summary += string.Format("AP: {0}/", ap);
                return summary;
            }
        }
#endif
    }
}