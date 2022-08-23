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
    using Battle;

    public class EnemySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private TMP_Text textName;
        [SerializeField]
        private TMP_Text textCount;

        private Button button;
        private ScrollRect scrollRect;

        private void Awake() {
            button = GetComponent<Button>();
        }

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

        public void Init(UnitData data, int count, ScrollRect scrollRect, Action onClick) {
            this.imageIcon.sprite = data.Icon;
            this.textName.text = data.Name;
            this.textCount.text = string.Format("x {0}", count);

            this.scrollRect = scrollRect;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}