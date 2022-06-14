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
    [CreateAssetMenu(fileName = "CookingLevelData", menuName = "B409/Cooking Level Data")]
    [InlineEditor]
    public class CookingLevelTable : ScriptableObject {
        [SerializeField]
        private List<CookingLevelData> datas;
        public List<CookingLevelData> Datas => datas;

        [Serializable]
        public class CookingLevelData {
            [FoldoutGroup("@Summary")]
            [HorizontalGroup("@Summary/group", 300f)]
            [BoxGroup("@Summary/group/Inquired Parameters")]
            [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
            [SerializeField]
            private List<ParameterValue> inquiredParameters = new List<ParameterValue>();
            [HorizontalGroup("@Summary/group")]
            [BoxGroup("@Summary/group/Cooking Datas")]
            [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
            [SerializeField]
            private List<CookingData> cookingDatas = new List<CookingData>();

            public List<ParameterValue> InquiredParameters => inquiredParameters;
            public List<CookingData> CookingDatas => cookingDatas;

#if UNITY_EDITOR
            private string Summary {
                get {
                    string summary = "";
                    foreach(var parameterValue in inquiredParameters) {
                        summary += string.Format("{0} {1}/", parameterValue.Parameter, parameterValue.Value);
                    }
                    summary += string.Format("Cooking Datas {0}", cookingDatas.Count);
                    return summary;
                }
            }
#endif
        }
    }
}