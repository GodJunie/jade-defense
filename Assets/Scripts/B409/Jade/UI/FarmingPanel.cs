using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace B409.Jade.UI {
    using Game;
    using Data;

    public class FarmingPanel : MonoBehaviour {
        [BoxGroup("General")]
        [SerializeField]
        private Text textName;
        [BoxGroup("General")]
        [SerializeField]
        private FarmingSlot farmSlotPrefab;
        [BoxGroup("General")]
        [SerializeField]
        private Transform farmContainer;

        [BoxGroup("Info Panel")]
        [SerializeField]
        private GameObject infoPanel;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private Text textFarmName;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private Text textFarmDescription;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private Transform itemContainer;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private ItemSlot itemSlotPrefab;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private Text textInquiredParameters;
        [BoxGroup("Info Panel")]
        [SerializeField]
        private Button buttonEnter;

        private FarmingLevelData data;

        public void Open(FarmingLevelTable table) {
            this.textName.text = table.Name;

            for(int i = 0; i < farmContainer.childCount; i++) {
                Destroy(farmContainer.GetChild(i).gameObject);
            }

            foreach(var data in table.Datas) {
                var slot = Instantiate(farmSlotPrefab, farmContainer);
                slot.Init(data, () => OpenInfoPanel(data));
            }

            this.gameObject.SetActive(true);
        }

        private void OpenInfoPanel(FarmingLevelData data) {
            this.data = data;

            this.textFarmName.text = data.Name;
            this.textFarmDescription.text = data.Description;

            for(int i = 0; i < itemContainer.childCount; i++) {
                Destroy(itemContainer.GetChild(i).gameObject);
            }

            foreach(var reward in data.Datas) {
                if(reward.Item == null) 
                    continue;
                var slot = Instantiate(itemSlotPrefab, itemContainer);
                slot.Init(reward.Item);
            }

            this.textInquiredParameters.text = GameManager.Instance.Progress.GetInquiredParametersText(data.InquiredParameters);

            this.buttonEnter.interactable = GameManager.Instance.Progress.CheckParameterValidation(data.InquiredParameters);

            this.infoPanel.SetActive(true);
        }

        public void OnEnterFarm() {
            GameManager.Instance.Farming(data);
            this.infoPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}