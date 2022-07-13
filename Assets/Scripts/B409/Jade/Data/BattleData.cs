using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Data {
    public class BattleData : ScriptableObject {
        [SerializeField]
        private float distance;


        public float Distance => distance;
    }
}