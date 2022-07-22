using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;

    [RequireComponent(typeof(Button))]
    public class RecipeItemSlot : ItemSlot, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
        [SerializeField]
        private GameObject lockObject;

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

        public void Init(ItemData data, int count, ScrollRect scrollRect, bool validation, Action onClick) {
            GetComponent<Button>().onClick.AddListener(() => onClick?.Invoke());
            this.scrollRect = scrollRect;
            this.Init(data, count);
            this.lockObject.SetActive(!validation);
        }
    }
}