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

    public class UnitData : ScriptableObject {
        [BoxGroup("Id")]
        [HideLabel]
        [SerializeField]
        private int id;

        [SerializeField]
        private GameObject prefab;
        [SerializeField]
        private new string name;
        [SerializeField]
        private string description;
        [SerializeField]
        private float cooltime;
        [SerializeField]
        private Sprite icon;

        [InlineEditor]
        [HideLabel]
        [SerializeField]
        private Status status;

        public int Id => id;
        public GameObject Prefab => prefab;
        public string Name => name;
        public string Description => description;
        public Status Status => status;
        public Sprite Icon => icon;
    }
}
