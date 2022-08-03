using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class MonsterBuySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private TMP_Text textName;
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private ItemSlot slotPrefab;

        private ScrollRect scrollRect;
        private MonsterData data;

        public void OnBeginDrag(PointerEventData e) {
            scrollRect.OnBeginDrag(e);
        }

        public void OnDrag(PointerEventData e) {
            scrollRect.OnDrag(e);
        }

        public void OnEndDrag(PointerEventData e) {
            scrollRect.OnEndDrag(e);
        }

        public void OnScroll(PointerEventData e) {
            scrollRect.OnScroll(e);
        }

        public void Init(MonsterData data, ScrollRect scrollRect, Action onClick) {
            this.data = data;
            this.scrollRect = scrollRect;

            imageIcon.sprite = data.Icon;
            textName.text = data.Name;
            GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());

            foreach(var item in data.Cost) {
                var slot = Instantiate(slotPrefab, slotContainer);
                slot.Init(item.Item, GameManager.Instance.Progress.GetItemCount(item.Item.Id), item.Count);
            }
        }
    }
}