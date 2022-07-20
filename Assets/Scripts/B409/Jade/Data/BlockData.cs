using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    [System.Serializable]
    public class BlockData {
        [SerializeField]
        private int day;
        [SerializeField]
        private BattleData battle;

        public int Day => day;
        public BattleData Battle => battle;
    }
}