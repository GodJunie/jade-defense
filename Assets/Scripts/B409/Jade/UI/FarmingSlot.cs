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

        public void Init(FarmingLevelData data, Action onClick) {
            GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());

            this.textName.text = data.Name;

            foreach(var itemData in data.Datas) {
                if(itemData.Item == null) continue;
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.Init(itemData.Item);
            }

            lockObject.SetActive
                (!GameManager.Instance.Progress.CheckParameterValidation(data.InquiredParameters));
        }
    }
}