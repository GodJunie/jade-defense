using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "BattleData", menuName = "B409/Battle Data")]
    public class BattleData : ScriptableObject, IDataID {
        [BoxGroup("ID")]
        [HideLabel]
        [SerializeField]
        private int id;
        [HorizontalGroup("group", .5f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<UnitData> enemies;
        [HorizontalGroup("group", .5f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<MaterialData> rewards;

        public int Id => id;
        public List<UnitData> Enemies => enemies;
        public List<MaterialData> Rewards => rewards;
    }
}