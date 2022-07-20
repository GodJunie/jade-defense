// System
using System;
using System.Collections;
using System.Collections.Generic;
// UnityEngine
using UnityEngine;
// OdinInspector
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    using Game;

    [CreateAssetMenu(fileName = "MonsterData", menuName = "B409/Monster Data")]
    public class MonsterData : UnitData {
        [BoxGroup("group2/Settings")]
        [SerializeField]
        private List<MaterialData> cost;
        
        public List<MaterialData> Cost => cost;
    }
}
