using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;

    public class InventoryItemSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerEnterHandler, IPointerExitHandler {
        private ScrollRect scrollRect;
        private Action onPointerEnter;
        private Action onPointerExit;

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

        public void OnPointerEnter(PointerEventData eventData) {
            this.onPointerEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData) {
            this.onPointerExit?.Invoke();
        }

        public void Init(ItemData data, int count, Action onPointerEnter, Action onPointerExit, ScrollRect scrollRect) {
            this.scrollRect = scrollRect;

            this.onPointerEnter = onPointerEnter;
            this.onPointerExit = onPointerExit;

            this.Init(data, count);
        }
    }
}