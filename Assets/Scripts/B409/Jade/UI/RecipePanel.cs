using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class RecipePanel : MonoBehaviour {
        [BoxGroup("Genereal")]
        [SerializeField]
        private Text textName;
        [BoxGroup("Genereal")]
        [SerializeField]
        private Transform itemContainer;
        [BoxGroup("Genereal")]
        [SerializeField]
        private RecipeItemSlot recipeItemSlotPrefab;
        [BoxGroup("Genereal")]
        [SerializeField]
        private ScrollRect itemScrollRect;

        [BoxGroup("Info")]
        [SerializeField]
        private GameObject infoPanel;
        [BoxGroup("Info")]
        [SerializeField]
        private Text textResult;
        [BoxGroup("Info")]
        [SerializeField]
        private ItemSlot resultItemSlot;
        [BoxGroup("Info")]
        [SerializeField]
        private Transform materialContainer;
        [BoxGroup("Info")]
        [SerializeField]
        private ItemSlot itemSlotPrefab;
        [BoxGroup("Info")]
        [SerializeField]
        private Text textParameters;

        private List<ParameterValue> inquiredParameters;
        private RecipeData data;

        public void Open(ScriptableObject table) {
            if(table is CookingLevelTable)
                this.Open<CookingLevelTable, CookingData>(table as CookingLevelTable);
            if(table is CraftingLevelTable)
                this.Open<CraftingLevelTable, CraftingData>(table as CraftingLevelTable);
            if(table is JewelleryLevelTable)
                this.Open<JewelleryLevelTable, JewelleryData>(table as JewelleryLevelTable);
        }

        public void Open<Table, Data>(Table table) where Table : RecipeLevelTable<Table, Data> where Data : RecipeData {
            this.textName.text = table.Name;

            for(int i = 0; i < itemContainer.childCount; i++) {
                Destroy(itemContainer.GetChild(i).gameObject);
            }

            foreach(var levelData in table.Datas) {
                bool validation = GameManager.Instance.Progress.CheckParameterValidation(levelData.InquiredParameters);

                foreach(var data in levelData.Datas) {
                    var slot = Instantiate(this.recipeItemSlotPrefab, this.itemContainer);
                    slot.Init(data.Result.Item, data.Result.Count, itemScrollRect, validation, () => {
                        this.inquiredParameters = levelData.InquiredParameters;
                        ShowInfo(data);
                    });
                }
            }

            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);
        }

        private void ShowInfo(RecipeData data) {
            this.data = data;

            this.textResult.text = data.Result.Item.Name;
            this.resultItemSlot.Init(data.Result.Item, data.Result.Count);

            for(int i = 0; i < materialContainer.childCount; i++) {
                Destroy(materialContainer.GetChild(i).gameObject);
            }

            foreach(var material in data.Materials) {
                var slot = Instantiate(this.itemSlotPrefab, materialContainer);
                slot.Init(material.Item, material.Count);
            }

            this.textParameters.text = GameManager.Instance.Progress.GetInquiredParametersText(inquiredParameters);

            this.infoPanel.SetActive(true);
        }
    }
}