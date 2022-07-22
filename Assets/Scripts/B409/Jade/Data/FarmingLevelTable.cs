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
    [CreateAssetMenu(fileName = "FarmingLevelTable", menuName = "B409/Farming Level Data")]
    public class FarmingLevelTable : ScriptableObject {
        [SerializeField]
        private new string name;
        [SerializeField]
        private List<FarmingLevelData> datas;

        public string Name => name;
        public List<FarmingLevelData> Datas => datas;
    }
}