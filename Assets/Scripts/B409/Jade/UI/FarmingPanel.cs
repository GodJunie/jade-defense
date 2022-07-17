using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.UI {
    using Game;
    using Data;
    using System.Linq;

    public class FarmingPanel : MonoBehaviour {
        [SerializeField]
        private FarmingLevelTable table;
        [SerializeField]
        private FarmingSlot slotPrefab;
        [SerializeField]
        private Transform slotContainer;

        private List<FarmingSlot> slots;

        private void Awake() {
            Init();
        }

        private void Init() {
            this.slots = new List<FarmingSlot>();
            foreach(var data in table.Datas) {
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.Init(data, 
                    () => { 
                        GameManager.Instance.Farming(data);
                        this.gameObject.SetActive(false);
                    }
                );
                slots.Add(slot);
            }
        }
    }
}