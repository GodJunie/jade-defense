using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class MonsterBuySlot : MonoBehaviour {
        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private Text textName;
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private ItemSlot slotPrefab;

        public void Init(MonsterData data) {
            imageIcon.sprite = data.Icon;
            textName.text = data.Name;

            foreach(var item in data.Cost) {
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.Init(item.Item, GameManager.Instance.Progress.GetItemCount(item.Item.Id), item.Count);
            }
        }
    }
}