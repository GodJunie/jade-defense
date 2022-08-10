using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;
    using Battle;

    public class MonsterBuyPanel : MonoBehaviour {
        [BoxGroup("Tab")]
        [SerializeField]
        private Button tabBuyMonster;
        [BoxGroup("Tab")]
        [SerializeField]
        private Button tabOwnedMonster;

        [BoxGroup("Buy")]
        [SerializeField]
        private GameObject panelBuyMonster;
        [BoxGroup("Buy")]
        [SerializeField]
        private Transform buySlotContainer;
        [BoxGroup("Buy")]
        [SerializeField]
        private MonsterBuySlot buySlotPrefab;
        [BoxGroup("Buy")]
        [SerializeField]
        private ScrollRect buyScrollRect;
        [BoxGroup("Buy")]
        [SerializeField]
        private Button buttonBuy;

        [BoxGroup("Owned")]
        [SerializeField]
        private GameObject panelOwnedMonster;
        [BoxGroup("Owned")]
        [SerializeField]
        private Transform ownedSlotContainer;
        [BoxGroup("Owned")]
        [SerializeField]
        private MonsterOwnedSlot ownedSlotPrefab;
        [BoxGroup("Owned")]
        [SerializeField]
        private ScrollRect ownedScrollRect;
        

        [BoxGroup("Info")]
        [SerializeField]
        private GameObject panelInfo;
        [BoxGroup("Info")]
        [SerializeField]
        private Image imageInfoIcon;

        [BoxGroup("Info")]
        [SerializeField]
        private TMP_Text textInfoName;

        [BoxGroup("Info")]
        [FoldoutGroup("Info/Status Text")]
        [SerializeField]
        private TMP_Text textCooltime;
        [FoldoutGroup("Info/Status Text")]
        [SerializeField]
        private TMP_Text textRange;
        [FoldoutGroup("Info/Status Text")]
        [SerializeField]
        private TMP_Text textMoveSpeed;
        [FoldoutGroup("Info/Status Text")]
        [SerializeField]
        private TMP_Text textHp;

        [BoxGroup("Info")]
        [SerializeField]
        private TMP_Text textAttackDescription;

        private MonsterData data;

        public void OpenBuyPanel() {
            this.gameObject.SetActive(true);
            panelInfo.SetActive(false);

            this.tabBuyMonster.interactable = false;
            this.tabOwnedMonster.interactable = true;

            this.panelBuyMonster.SetActive(true);
            this.panelOwnedMonster.SetActive(false);

            var progress = GameManager.Instance.Progress;
            var stage = DataManager.Instance.Stages[progress.Stage];
            var stageSequence = stage.Datas[progress.StageSequence];
            if(!(stageSequence is DailyRoutineData)) {
                return;
            }
            
            var monsterOnSale = (stageSequence as DailyRoutineData).Monsters;
            
            for(int i = 0; i < buySlotContainer.childCount; i++) {
                Destroy(this.buySlotContainer.GetChild(i).gameObject);
            }
            foreach(var monster in monsterOnSale) {
                var slot = Instantiate(buySlotPrefab, buySlotContainer);
                slot.Init(monster, buyScrollRect, () => {
                    this.ShowInfo(monster, true);
                });
            }
        }

        public void OpenOwnedPanel() {
            this.gameObject.SetActive(true);
            panelInfo.SetActive(false);

            this.tabBuyMonster.interactable = true;
            this.tabOwnedMonster.interactable = false;

            this.panelBuyMonster.SetActive(false);
            this.panelOwnedMonster.SetActive(true);


            for(int i = 0; i < ownedSlotContainer.childCount; i++) {
                Destroy(this.ownedSlotContainer.GetChild(i).gameObject);
            }
            foreach(var pair in GameManager.Instance.Progress.Monsters) {
                var data = DataManager.Instance.Monsters.Find(e => e.Id == pair.Key);

                var slot = Instantiate(ownedSlotPrefab, ownedSlotContainer);

                slot.Init(data, pair.Value, ownedScrollRect, () => {
                    this.ShowInfo(data, false);
                });
            }
        }

        [Button]
        private void ShowInfo(MonsterData data, bool buy) {
            this.data = data;

            this.buttonBuy.gameObject.SetActive(buy);
            this.buttonBuy.interactable = GameManager.Instance.Progress.CheckItemsEnough(data.Cost);

            // Info
            this.imageInfoIcon.sprite = data.Icon;

            this.textInfoName.text = data.Name;

            this.textCooltime.text = string.Format("{0:0.#}", data.Status.Cooltime);
            this.textRange.text = data.Status.Range.ToString("0.#");
            this.textHp.text = data.Status.Hp.ToString("0");
            this.textMoveSpeed.text = data.Status.MoveSpeed.ToString("0");

            this.textAttackDescription.text = data.Status.GetAttackDescriptionString();

            panelInfo.SetActive(true);
        }

        public void Buy() {
            GameManager.Instance.BuyMonster(this.data);
            this.OpenBuyPanel();
            this.ShowInfo(this.data, true);
        }
    }
}