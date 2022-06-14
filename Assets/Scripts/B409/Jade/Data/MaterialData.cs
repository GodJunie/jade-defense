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
    [Serializable]
    public class MaterialData {
        [HorizontalGroup("group")]
        [BoxGroup("group/Item")]
        [HideLabel]
        [SerializeField]
        private ItemData item;
        [HorizontalGroup("group", 100f)]
        [BoxGroup("group/Count")]
        [HideLabel]
        [SerializeField]
        private int count;

        public ItemData Item => item;
        public int Count => count;
    }
}