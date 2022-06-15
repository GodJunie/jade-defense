// System
using System;
using System.Collections;
using System.Collections.Generic;
// UnityEngine
using UnityEngine;
// OdinInspector
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "ItemData", menuName = "B409/Item Data")]
    public class ItemData : ScriptableObject {
        [HorizontalGroup("group", 150f)]
        [BoxGroup("group/Icon")]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 150f)]
        [HideLabel]
        [SerializeField]
        private Sprite icon;

        [VerticalGroup("group/group")]
        [BoxGroup("group/group/ID")]
        [HideLabel]
        [SerializeField]
        private int id;

        [VerticalGroup("group/group")]
        [BoxGroup("group/group/Name")]
        [HideLabel]
        [SerializeField]
        private new string name;

        [VerticalGroup("group/group")]
        [BoxGroup("group/group/Description")]
        [TextArea(5, 5)]
        [HideLabel]
        [SerializeField]
        private string description;

        public int Id => id;
        public string Name => name;
        public string Descriptoin => description;
        public Sprite Icon => icon;
    }
}