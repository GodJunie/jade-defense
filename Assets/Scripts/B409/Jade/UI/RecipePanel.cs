using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class RecipePanel : MonoBehaviour {
        [BoxGroup("Connect")]
        [SerializeField]
        private MainScreen mainScreen;

        [BoxGroup("Genereal")]
        [SerializeField]
        private TMP_Text textName;
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
        private TMP_Text textResult;
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
        private TMP_Text textParameters;
        [BoxGroup("Info")]
        [SerializeField]
        private Button buttonConfirm;

        private ActionLevelData levelData;
        private RecipeData data;

        public void Open(ScriptableObject table) {
            if(table is CookingLevelTable)
                this.Open<CookingLevelTable, CookingLevelData, CookingData>(table as CookingLevelTable);
            if(table is CraftingLevelTable)
                this.Open<CraftingLevelTable, CraftingLevelData, CraftingData>(table as CraftingLevelTable);
            if(table is JewelleryLevelTable)
                this.Open<JewelleryLevelTable, JewelleryLevelData, JewelleryData>(table as JewelleryLevelTable);
        }

        public void Open<Table, LevelData, Data>(Table table) where Table : RecipeLevelTable<Table, LevelData, Data> where LevelData : RecipeLevelData<LevelData, Data> where Data : RecipeData {
            this.textName.text = table.Name;

            for(int i = 0; i < itemContainer.childCount; i++) {
                Destroy(itemContainer.GetChild(i).gameObject);
            }

            foreach(var levelData in table.Datas) {
                bool validation = GameManager.Instance.Progress.CheckParameterValidation(levelData.InquiredParameters);

                foreach(var data in levelData.Datas) {
                    var slot = Instantiate(this.recipeItemSlotPrefab, this.itemContainer);
                    slot.Init(data.Result.Item, data.Result.Count, itemScrollRect, validation, () => {
                        this.levelData = levelData;
                        ShowInfo(data);
                    });
                }
            }

            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);
        }

        private void ShowInfo(RecipeData data) {
            this.data = data;
            bool validation = GameManager.Instance.Progress.CheckParameterValidation(this.levelData.InquiredParameters);

            this.textResult.text = data.Result.Item.Name;
            this.resultItemSlot.Init(data.Result.Item, data.Result.Count);

            for(int i = 0; i < materialContainer.childCount; i++) {
                Destroy(materialContainer.GetChild(i).gameObject);
            }

            foreach(var material in data.Materials) {
                var slot = Instantiate(this.itemSlotPrefab, materialContainer);
                slot.Init(material.Item, GameManager.Instance.Progress.GetItemCount(material.Item.Id), material.Count);
            }

            this.textParameters.text = GameManager.Instance.Progress.GetInquiredParametersText(this.levelData.InquiredParameters);

            this.buttonConfirm.interactable = validation;
            this.infoPanel.SetActive(true);
        }

        public void OnConfirm() {
            GameManager.Instance.Making(this.levelData, this.data);
            mainScreen.Init();
            this.infoPanel.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}