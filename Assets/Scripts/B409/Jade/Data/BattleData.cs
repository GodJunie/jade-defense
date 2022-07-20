using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "BattleData", menuName = "B409/Battle Data")]
    public class BattleData : ScriptableObject {
        [SerializeField]
        private List<UnitData> enemies;
        [SerializeField]
        private List<ItemData> rewards;

        public List<UnitData> Enemies => enemies;
        public List<ItemData> Rewards => rewards;
    }
}