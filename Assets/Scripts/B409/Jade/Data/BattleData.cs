using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "BattleData", menuName = "B409/Battle Data")]
    public class BattleData : ScriptableObject, IDataID {
        [SerializeField]
        private int id;
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<UnitData> enemies;
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<ItemData> rewards;

        public int Id => id;
        public List<UnitData> Enemies => enemies;
        public List<ItemData> Rewards => rewards;
    }
}