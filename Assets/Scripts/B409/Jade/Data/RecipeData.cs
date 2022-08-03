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
    public abstract class RecipeData : ScriptableObject, IDataID {
        [BoxGroup("Id")]
        [HideLabel]
        [SerializeField]
        private int id;

        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Recipe")]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<MaterialData> materials;
        [HorizontalGroup("group", .5f)]
        [BoxGroup("group/Result")]
        [HideLabel]
        [SerializeField]
        private MaterialData result;
       
        public int Id => id;
        public List<MaterialData> Materials => materials;
        public MaterialData Result => result;
    }
}