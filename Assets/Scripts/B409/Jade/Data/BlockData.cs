using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [System.Serializable]
    public class BlockData {
        [FoldoutGroup("@Summary")]
        [HorizontalGroup("@Summary/group", .5f)]
        [VerticalGroup("@Summary/group/group")]
        [BoxGroup("@Summary/group/group/Day")]
        [HideLabel]
        [SerializeField]
        private int day;
        [VerticalGroup("@Summary/group/group")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<MonsterData> monsters;

        [HorizontalGroup("@Summary/group")]
        [InlineEditor(Expanded = true)]
        [SerializeField]
        private BattleData battle;
    

        public int Day => day;
        public BattleData Battle => battle;
        public List<MonsterData> Monsters => monsters;

#if UNITY_EDITOR
        private string Summary {
            get {
                return string.Format("Day: {0}, Monsters: {1}, Battle Id: {2}", this.day, this.monsters?.Count, this.battle.name);
            }
        }
#endif
    }
}