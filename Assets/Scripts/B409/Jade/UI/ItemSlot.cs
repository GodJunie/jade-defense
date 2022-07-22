using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class ItemSlot : MonoBehaviour {
        [SerializeField]
        private Text textCount;
        [SerializeField]
        private Image imageIcon;

        public void Init(Sprite sprite, int count = 0, int maxCount = 0) {
            this.imageIcon.sprite = sprite;

            if(maxCount > 0) {
                // 기준이 있음
                this.textCount.text = string.Format("<color={0}>{1}</color>/{2}", count < maxCount ? "red" : "green", count, maxCount);
            } else {
                // 기준 없음
                if(count > 0)
                    this.textCount.text = count.ToString("N0");
                else
                    this.textCount.text = "";
            }
        }

        public void Init(ItemData data, int count = 0, int maxCount = 0) {
            this.Init(data.Icon, count, maxCount);
        }

        public void Init(int id, int count = 0, int maxCount = 0) {
            var item = DataManager.Instance.Items.Find(e => e.Id == id);
            if(item == null)
                throw new System.Exception(string.Format("There is no item id : {0}", id));

            this.Init(item, count, maxCount);
        }
    }
}