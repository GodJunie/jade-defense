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
    [Serializable]
    public class FarmingLevelData {
        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Info")]
        [HorizontalGroup("@Summary/Info/group", 300f)]
        [BoxGroup("@Summary/Info/group/Name")]
        [HideLabel]
        [SerializeField]
        private string name;

        [FoldoutGroup("@Summary")]
        [HorizontalGroup("@Summary/Info/group")]
        [BoxGroup("@Summary/Info/group/Description")]
        [TextArea(0, 3)]
        [HideLabel]
        [SerializeField]
        private string description;

        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Data")]
        [HorizontalGroup("@Summary/Data/group", 300f)]
        [VerticalGroup("@Summary/Data/group/group")]
        [BoxGroup("@Summary/Data/group/group/Inquired Parameters")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ParameterValue> inquiredParameters = new List<ParameterValue>();

        [BoxGroup("@Summary/Data/group/group/Reward Parameters")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ParameterValue> rewardParameters = new List<ParameterValue>();

        [BoxGroup("@Summary/Data/group/group/AP")]
        [HideLabel]
        [SerializeField]
        private float ap;


        [HorizontalGroup("@Summary/Data/group")]
        [BoxGroup("@Summary/Data/group/Datas")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ItemRateData> datas = new List<ItemRateData>();

        public string Name => name;
        public string Description => description;
        public List<ParameterValue> InquiredParameters => inquiredParameters;
        public List<ParameterValue> RewardParameters => rewardParameters;
        public float AP => ap;
        public List<ItemRateData> Datas => datas;

        [Serializable]
        public class ItemRateData {
            [HorizontalGroup("group")]
            [BoxGroup("group/Item")]
            [HideLabel]
            [SerializeField]
            private ItemData item;
            [HorizontalGroup("group", 100f)]
            [BoxGroup("group/Rate")]
            [HideLabel]
            [SerializeField]
            private float rate;

            public ItemData Item => item;
            public float Rate => rate;
        }

#if UNITY_EDITOR
        private string Summary {
            get {
                string summary = name;
                foreach(var parameterValue in inquiredParameters) {
                    summary += string.Format("/{0} {1}/", parameterValue.Parameter, parameterValue.Value);
                }
                summary += string.Format("Datas {0}", datas.Count);
                return summary;
            }
        }
#endif
    }
}