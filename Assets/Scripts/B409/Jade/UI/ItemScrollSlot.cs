using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;

    public class ItemScrollSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        private ScrollRect scrollRect;

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

        public void Init(ItemData data, int count, ScrollRect scrollRect, Action onClick) {
            this.Init(data, count);
            this.scrollRect = scrollRect;
            GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());
        }
    }
}