using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    public class BattleData : ScriptableObject {
        [SerializeField]
        private List<EnemyData> enemies;
        [SerializeField]
        private List<ItemData> rewards;

        public List<EnemyData> Enemies;
        public List<ItemData> Rewards;
    }
}