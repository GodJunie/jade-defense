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
    public class FarmingLevelData : ActionLevelData {
        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Farming Info")]
        [HorizontalGroup("@Summary/Farming Info/group", 300f)]
        [BoxGroup("@Summary/Farming Info/group/Name")]
        [HideLabel]
        [SerializeField]
        private string name;

        [HorizontalGroup("@Summary/Farming Info/group")]
        [BoxGroup("@Summary/Farming Info/group/Description")]
        [TextArea(0, 3)]
        [HideLabel]
        [SerializeField]
        private string description;

        [FoldoutGroup("@Summary")]
        [BoxGroup("@Summary/Items")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ItemRateData> datas = new List<ItemRateData>();

        public string Name => name;
        public string Description => description;
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