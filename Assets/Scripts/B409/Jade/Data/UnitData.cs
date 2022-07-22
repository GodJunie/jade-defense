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
    using Battle;

    [CreateAssetMenu(fileName = "UnitData", menuName = "B409/Unit Data")]
    public class UnitData : ScriptableObject, IDataID {
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

        [HorizontalGroup("group2", .5f)]
        [BoxGroup("group2/Settings")]
        [BoxGroup("group2/Settings/Prefab")]
        [HideLabel]
        [SerializeField]
        private UnitController prefab;

        [HorizontalGroup("group2", .5f)]
        [BoxGroup("group2/Status")]
        [HideLabel]
        [SerializeField]
        private Status status;

        public int Id => id;
        public string Name => name;
        public string Description => description;
     
        public Sprite Icon => icon;
        public UnitController Prefab => prefab;

        public Status Status => status;
    }
}
