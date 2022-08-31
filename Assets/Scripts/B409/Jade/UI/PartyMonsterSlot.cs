using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;

    public class PartyMonsterSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        [SerializeField]
        private TMP_Text textIndex;
        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private UnitStatus unitStatus;

        public MonsterData Data { get; private set; }

        private ScrollRect scrollRect;
        private Action moveUp;
        private Action moveDown;

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

        public void Init(MonsterData data, ScrollRect scrollRect, Action action, Action moveUp, Action moveDown) {
            this.Data = data;
            this.scrollRect = scrollRect;
            this.imageIcon.sprite = data.Icon;
            this.unitStatus.SetUI(data.Status);

            var button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action?.Invoke());

            this.moveUp = moveUp;
            this.moveDown = moveDown;
        }

        public void SetIndex(int index) {
            transform.SetSiblingIndex(index);
            this.textIndex.text = (index + 1).ToString();
        }

        public void MoveUp() {
            moveUp?.Invoke();
        }

        public void MoveDown() {
            moveDown?.Invoke();
        }
    }
}