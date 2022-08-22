using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Game;
    using Data;

    public class InventoryPanel : MonoBehaviour {
        [SerializeField]
        private InventoryItemSlot slotPrefab;
        [SerializeField]
        private Transform container;
        [SerializeField]
        private ScrollRect scrollRect;

        [SerializeField]
        private RectTransform tooltipPivot;
        [SerializeField]
        private RectTransform tooltipBackground;
        [SerializeField]
        private Text tooltipText;
        [SerializeField]
        private Vector2 tooltipPadding;


        private void Update() {
            if(tooltipPivot.gameObject.activeInHierarchy) {
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0;

                this.tooltipPivot.position = worldPos;
            }
        }

        private void OnEnable() {
            var items = GameManager.Instance.Progress.Items;
            tooltipPivot.gameObject.SetActive(false);

            for(int i = 0; i < container.childCount; i++) {
                Destroy(container.GetChild(i).gameObject);
            }

            foreach(var item in items) {
                int id = item.Key;
                int count = item.Value;

                Debug.Log(string.Format("id: {0}, count: {1}", id, count));

                var data = DataManager.Instance.Items.Find(e => e.Id == id);

                if(!data.Inventory)
                    continue;

                var slot = Instantiate(slotPrefab, this.container);

                slot.Init(data, count,
                    () => {
                        this.tooltipPivot.gameObject.SetActive(true);
                        this.tooltipText.text = data.Name;

                        var size = new Vector2(tooltipText.preferredWidth, tooltipText.preferredHeight);
                        tooltipText.rectTransform.sizeDelta = size;

                        this.tooltipBackground.sizeDelta = size + tooltipPadding;
                    },
                    () => {
                        this.tooltipPivot.gameObject.SetActive(false);
                    },
                    scrollRect
                );
            }
        }
    }
}