using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace B409.Jade.UI {
    public class MonsterBuyPanel : MonoBehaviour {
        [SerializeField]
        private Transform buySlotContainer;
        [SerializeField]
        private MonsterBuySlot buySlotPrefab;

        public void Open() {
            OpenBuyPanel();
        }

        public void OpenBuyPanel() {

        }

        public void OpenOwnedPanel() {

        }
    }
}