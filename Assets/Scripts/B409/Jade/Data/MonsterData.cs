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
    public class MonsterData : ScriptableObject {
        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private new string name;
        [SerializeField]
        private string description;
        [SerializeField]
        private float cooltime;

        [InlineEditor]
        [SerializeField]
        private Status status;
    }
}
