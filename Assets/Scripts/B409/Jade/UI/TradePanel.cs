using System;
using System.Linq;
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

    public class TradePanel : MonoBehaviour {
        [BoxGroup("General")]
        [SerializeField]
        private TMP_Text textGold;


        [BoxGroup("Tab")]
        [SerializeField]
        private Button tabBuy;
        [BoxGroup("Tab")]
        [SerializeField]
        private Button tabSell;


        [BoxGroup("Info")]
        [SerializeField]
        private GameObject infoPanel;
        [BoxGroup("Info")]
        [SerializeField]
        private TMP_Text textName;
        [BoxGroup("Info")]
        [FoldoutGroup("Info/Item")]
        [SerializeField]
        private GameObject itemInfo;
        [FoldoutGroup("Info/Item")]
        [SerializeField]
        private Image imageItem;
        [FoldoutGroup("Info/Item")]
        [SerializeField]
        private TMP_Text textItemDescription;

        [BoxGroup("Info")]
        [FoldoutGroup("Info/Monster")]
        [SerializeField]
        private GameObject monsterInfo;
        [FoldoutGroup("Info/Monster")]
        [SerializeField]
        private Image imageMonster;
        [FoldoutGroup("Info/Monster")]
        [FoldoutGroup("Info/Monster/Status Text")]
        [SerializeField]
        private TMP_Text textCooltime;
        [FoldoutGroup("Info/Monster/Status Text")]
        [SerializeField]
        private TMP_Text textRange;
        [FoldoutGroup("Info/Monster/Status Text")]
        [SerializeField]
        private TMP_Text textMoveSpeed;
        [FoldoutGroup("Info/Monster/Status Text")]
        [SerializeField]
        private TMP_Text textHp;
        [FoldoutGroup("Info/Monster")]
        [SerializeField]
        private TMP_Text textMonsterDescription;

        [BoxGroup("Info")]
        [SerializeField]
        private Button buttonConfirm;
        [BoxGroup("Info")]
        [SerializeField]
        private TMP_Text textPrice;

        [BoxGroup("Buy")]
        [SerializeField]
        private GameObject buyPanel;
        [BoxGroup("Buy")]
        [SerializeField]
        private Transform buySlotContainer;
        [BoxGroup("Buy")]
        [SerializeField]
        private ScrollRect buyScrollRect;
        [BoxGroup("Buy")]
        [SerializeField]
        private TradeBuySlot buySlotPrefab;
        [BoxGroup("Buy")]
        [SerializeField]
        private Button buttonRefresh;
        [BoxGroup("Buy")]
        [SerializeField]
        private TMP_Text textRefresh;

        [BoxGroup("Sell")]
        [SerializeField]
        private GameObject sellPanel;


        private bool sellViewIsGrid;
        private bool sellViewItems;
        private bool sellViewMonsters;

        private List<TradeBuySlot> buySlotPool = new List<TradeBuySlot>();

        private ScriptableObject data;
        private bool buy;


        public void OpenBuyPanel() {
            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);

            tabBuy.interactable = false;
            tabSell.interactable = true;

            buyPanel.SetActive(true);
            sellPanel.SetActive(false);

            var progress = GameManager.Instance.Progress;

            foreach(var slot in buySlotPool) {
                slot.gameObject.SetActive(false);
            }

            Debug.Log(progress.Trades.Count);

            foreach(var pair in progress.Trades) {
                int id = pair.Key;
                int count = pair.Value;

                var slot = GetBuySlot();

                var item = DataManager.Instance.Items.Find(e => e.Id == id);
                if(item != null) {
                    slot.Init(item, count, buyScrollRect, () => {
                        ShowInfo(item, true);
                    });
                    continue;
                }

                var monster = DataManager.Instance.Monsters.Find(e => e.Id == id);
                if(monster != null) {
                    slot.Init(monster, count, buyScrollRect, () => {
                        ShowInfo(monster, true);
                    });
                    continue;
                }
            }

            this.buttonRefresh.image.color = progress.RefreshCount > 0 ? GameConsts.ButtonGreen : GameConsts.ButtonRed;
            this.textRefresh.text = progress.RefreshCount.ToString();
            this.buttonRefresh.interactable = progress.RefreshCount > 0;

            this.textGold.text = progress.Gold.ToString("N0");

            OrderBuySlots();
        }

        private TradeBuySlot GetBuySlot() {
            var slot = buySlotPool.Where(e => !e.gameObject.activeInHierarchy).FirstOrDefault();

            if(slot == default(TradeBuySlot)) {
                slot = Instantiate(buySlotPrefab, buySlotContainer);
                buySlotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            return slot;
        }

        private void OrderBuySlots() {
            var progress = GameManager.Instance.Progress;

            var slots = buySlotPool.Where(e => e.gameObject.activeInHierarchy).OrderByDescending(e => progress.Trades[e.Id] > 0).ThenBy(e => e.IsItem).ThenByDescending(e => e.Price).ToList();

            for(int i = 0; i < slots.Count; i++) {
                slots[i].transform.SetSiblingIndex(i);
            }
        }

        public void OpenSellPanel() {
            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);

            tabBuy.interactable = true;
            tabSell.interactable = false;

            buyPanel.SetActive(false);
            sellPanel.SetActive(true);

            var progress = GameManager.Instance.Progress;

            this.textGold.text = progress.Gold.ToString("N0");
        }

        private void ShowInfo(ScriptableObject data, bool buy) {
            this.data = data;
            this.buy = buy;

            this.infoPanel.SetActive(true);

            if(data is ItemData) {
                ShowItemInfo(data as ItemData);
            } else if(data is MonsterData) {
                ShowMonsterInfo(data as MonsterData);
            } else {
                throw new Exception("data is not item or monster");
            }

            var sale = data as ISale;

            if(buy) {
                buttonConfirm.image.color = GameConsts.ButtonGreen;
                textPrice.text = sale.BuyPrice.ToString("N0");
                if(sale.BuyPrice > GameManager.Instance.Progress.Gold) {
                    buttonConfirm.interactable = false;
                    textPrice.color = Color.red;
                } else {
                    buttonConfirm.interactable = true;
                    textPrice.color = Color.white;
                }
            } else {
                buttonConfirm.image.color = GameConsts.ButtonRed;
                textPrice.text = sale.SellPrice.ToString("N0");
                textPrice.color = Color.white;
            }
        }

        private void ShowItemInfo(ItemData data) {
            itemInfo.SetActive(true);
            monsterInfo.SetActive(false);

            this.imageItem.sprite = data.Icon;

            this.textName.text = data.Name;

            this.textItemDescription.text = data.Descriptoin;
        }

        private void ShowMonsterInfo(MonsterData data) {
            itemInfo.SetActive(false);
            monsterInfo.SetActive(true);

            this.imageMonster.sprite = data.Icon;

            this.textName.text = data.Name;

            this.textCooltime.text = string.Format("{0:0.#}", data.Status.Cooltime);
            this.textRange.text = data.Status.Range.ToString("0.#");
            this.textHp.text = data.Status.Hp.ToString("0");
            this.textMoveSpeed.text = data.Status.MoveSpeed.ToString("0");

            this.textMonsterDescription.text = data.Status.GetAttackDescriptionString();
        }

        public void RefreshBuyList() {
            var progress = GameManager.Instance.Progress;
            progress.RefreshTrades(GameManager.Instance.CurrentStageSequence as DailyRoutineData);

            OpenBuyPanel();
        }

        public void Confirm() {
            var progress = GameManager.Instance.Progress;

            var id = (this.data as IDataID).Id;
            var sale = this.data as ISale;

            if(this.buy) {
                if(data is ItemData) {
                    progress.AddItem(id, 1);
                } else if(data is MonsterData) {
                    progress.AddMonster(id, 1);
                }
                    
                progress.UseItem(GameConsts.GOLD_ID, sale.BuyPrice);
                progress.Trade(id);

                OpenBuyPanel();
            } else {
                if(data is ItemData) {
                    progress.UseItem(id, 1);
                } else if(data is MonsterData) {
                    progress.UseMonster(id, 1);
                }

                progress.AddItem(GameConsts.GOLD_ID, sale.SellPrice);

                OpenSellPanel();
            }
        }
    }
}
