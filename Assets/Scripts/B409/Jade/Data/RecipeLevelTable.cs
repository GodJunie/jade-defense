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
    [InlineEditor]
    public abstract class RecipeLevelTable<Table, Data> : ScriptableObject where Table : RecipeLevelTable<Table, Data> where Data : RecipeData {
        [SerializeField]
        private new string name;
        [SerializeField]
        private List<LevelData> datas;

        public string Name => name;
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
            private List<Data> datas = new List<Data>();

            public List<ParameterValue> InquiredParameters => inquiredParameters;
            public List<Data> Datas => datas;

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