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
    [CreateAssetMenu(fileName = "FarmingLevelTable", menuName = "B409/Farming Level Data")]
    [InlineEditor]
    public class FarmingLevelTable : ScriptableObject {
        [SerializeField]
        private List<LevelData> datas;
        public List<LevelData> Datas => datas;

        [Serializable]
        public class LevelData {
            [FoldoutGroup("@Summary")]
            [HorizontalGroup("@Summary/group", 300f)]
            [BoxGroup("@Summary/group/Inquired Parameters")]
            [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
            [SerializeField]
            private List<ParameterValue> inquiredParameters = new List<ParameterValue>();

            [HorizontalGroup("@Summary/group")]
            [BoxGroup("@Summary/group/Datas")]
            [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
            [SerializeField]
            private List<ItemRateData> datas = new List<ItemRateData>();


            public List<ParameterValue> InquiredParameters => inquiredParameters;
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
                    string summary = "";
                    foreach(var parameterValue in inquiredParameters) {
                        summary += string.Format("{0} {1}/", parameterValue.Parameter, parameterValue.Value);
                    }
                    summary += string.Format("Datas {0}", datas.Count);
                    return summary;
                }
            }
#endif
        }
    }
}