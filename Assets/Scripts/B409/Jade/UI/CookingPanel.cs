using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class CookingPanel : MonoBehaviour {
        [SerializeField]
        private CookingLevelTable table;




        private void Init() {
            foreach(var data in table.Datas) {

            }
        }
    }
}