using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Game;
    using Data;

    public class FarmingSlot : MonoBehaviour {
        [SerializeField]
        private ItemSlot slotPrefab;
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private Text textName;
        [SerializeField]
        private GameObject lockObject;

        private FarmingLevelTable.LevelData data;

        public void Init(FarmingLevelTable.LevelData data, Action onClick) {
            GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());

            this.data = data;

            this.textName.text = data.Name;

            foreach(var itemData in data.Datas) {
                if(itemData.Item == null) continue;
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.Init(itemData.Item);
            }

            CheckVailable();
        }

        private void OnEnable() {
            if(this.data == null) return;

            CheckVailable();
        }

        private void CheckVailable() {

            bool vailable = true;
            foreach(var parameterValue in this.data.InquiredParameters) {
                if(GameManager.Instance.Progress.Parameters[parameterValue.Parameter] < parameterValue.Value)
                    vailable = false;
            }

            this.lockObject.SetActive(!vailable);
        }
    }
}