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
    public abstract class RecipeLevelTable<Table, LevelData, Data> : ScriptableObject where Table : RecipeLevelTable<Table, LevelData, Data> where LevelData : RecipeLevelData<LevelData, Data> where Data : RecipeData {
        [SerializeField]
        private new string name;
        [SerializeField]
        private List<RecipeLevelData<LevelData, Data>> datas;

        public string Name => name;
        public List<RecipeLevelData<LevelData, Data>> Datas => datas;
    }

    [Serializable]
    public class RecipeLevelData<LevelData, Data> : ActionLevelData where LevelData : RecipeLevelData<LevelData, Data> where Data : RecipeData {
        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Items")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<Data> datas = new List<Data>();

        public List<Data> Datas => datas;
     

#if UNITY_EDITOR
        protected override string Summary {
            get {
                string summary = base.Summary;
                summary += string.Format("Datas {0}", datas.Count);
                return summary;
            }
        }
#endif
    }
}