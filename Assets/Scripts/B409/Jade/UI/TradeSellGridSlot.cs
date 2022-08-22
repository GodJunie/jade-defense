using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    public class TradeSellGridSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        [SerializeField]
        private GameObject itemSlot;
        [SerializeField]
        private GameObject monsterSlot;
        [SerializeField]
        private Image imageItem;
        [SerializeField]
        private Image imageMonster;
        [SerializeField]
        private TMP_Text textAmount;

        private Button button;
        private ScrollRect scrollRect;

        public int Id { get; private set; }
        public bool IsItem { get; private set; }

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

        public void Init(ScriptableObject data, int count, ScrollRect scrollRect, Action onClick) {
            var button = GetComponent<Button>();

            if(data is ItemData) {
                var item = data as ItemData;

                this.IsItem = true;

                this.itemSlot.SetActive(true);
                this.monsterSlot.SetActive(false);

                this.imageItem.sprite = item.Icon;

                this.Id = item.Id;
            } else if(data is MonsterData) {
                var monster = data as MonsterData;

                this.IsItem = false;

                this.monsterSlot.SetActive(true);
                this.itemSlot.SetActive(false);

                this.imageMonster.sprite = monster.Icon;

                this.Id = monster.Id;
            } else {
                throw new Exception(string.Format("data is not item or monster"));
            }

            this.textAmount.text = count.ToString();

            this.scrollRect = scrollRect;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}