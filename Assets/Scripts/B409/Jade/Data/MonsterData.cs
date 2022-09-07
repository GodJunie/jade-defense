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
    public class MonsterData : UnitData, ISale {
        [BoxGroup("group2/Settings")]
        [ListDrawerSettings(Expanded = true)]
        [SerializeField]
        private List<MaterialData> cost;

        [BoxGroup("group2/Settings")]
        [BoxGroup("group2/Settings/Price")]
        [BoxGroup("group2/Settings/Price/Buy")]
        [HideLabel]
        [SerializeField]
        private int buyPrice;
        [BoxGroup("group2/Settings/Price/Sell")]
        [HideLabel]
        [SerializeField]
        private int sellPrice;

        [BoxGroup("group2/Settings/IsUnique")]
        [HideLabel]
        [SerializeField]
        private bool isUnique = false;

        public int BuyPrice => buyPrice;
        public int SellPrice => sellPrice;
        public List<MaterialData> Cost => cost;
        public bool IsUnique => isUnique;
    }
}
