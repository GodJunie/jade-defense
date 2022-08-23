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
        [SerializeField]
        private UnitStatus unitStatus;

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

        [BoxGroup("Sell")]
        [FoldoutGroup("Sell/Grid")]
        [SerializeField]
        private ScrollRect sellGridScrollRect;
        [FoldoutGroup("Sell/Grid")]
        [SerializeField]
        private Transform sellGridSlotContainer;
        [FoldoutGroup("Sell/Grid")]
        [SerializeField]
        private TradeSellGridSlot sellGridSlotPrefab;

        [BoxGroup("Sell")]
        [FoldoutGroup("Sell/List")]
        [SerializeField]
        private ScrollRect sellListScrollRect;
        [FoldoutGroup("Sell/List")]
        [SerializeField]
        private Transform sellListSlotContainer;
        [FoldoutGroup("Sell/List")]
        [SerializeField]
        private TradeSellListSlot sellListSlotPrefab;


        private bool sellViewIsGridMode;

        private List<TradeBuySlot> buySlotPool = new List<TradeBuySlot>();
        private List<TradeSellGridSlot> sellGridSlotPool = new List<TradeSellGridSlot>();
        private List<TradeSellListSlot> sellListSlotPool = new List<TradeSellListSlot>();

        private ScriptableObject data;
        private bool buy;

        private void Fit() {
            foreach(var fitter in this.GetComponentsInChildren<ContentSizeFitter>()) {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
            }
        }

        #region Buy
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

            Fit();
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

        public void RefreshBuyList() {
            var progress = GameManager.Instance.Progress;
            progress.RefreshTrades(GameManager.Instance.CurrentStageSequence as DailyRoutineData);

            OpenBuyPanel();
        }
        #endregion

        #region Sell
        public void OpenGridSellPanel() {
            this.sellViewIsGridMode = true;

            sellGridScrollRect.gameObject.SetActive(true);
            sellListScrollRect.gameObject.SetActive(false);

            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);

            tabBuy.interactable = true;
            tabSell.interactable = false;

            buyPanel.SetActive(false);
            sellPanel.SetActive(true);

            var progress = GameManager.Instance.Progress;

            foreach(var slot in sellGridSlotPool) {
                slot.gameObject.SetActive(false);
            }

            Debug.Log(progress.Trades.Count);

            foreach(var pair in progress.Items) {
                int id = pair.Key;
                int count = pair.Value;

                var item = DataManager.Instance.Items.Find(e => e.Id == id);

                if(!item.CanSell)
                    continue;

                var gridSlot = GetSellGridSlot();

                gridSlot.Init(item, count, sellGridScrollRect, () => {
                    ShowInfo(item, false);
                });
            }

            foreach(var pair in progress.Monsters) {
                int id = pair.Key;
                int count = pair.Value;

                var gridSlot = GetSellGridSlot();

                var monster = DataManager.Instance.Monsters.Find(e => e.Id == id);

                gridSlot.Init(monster, count, sellGridScrollRect, () => {
                    ShowInfo(monster, false);
                });
            }

            this.textGold.text = progress.Gold.ToString("N0");

            OrderGridSellSlots();

            Fit();
        }

        public void OpenListSellPanel() {
            this.sellViewIsGridMode = false;

            sellGridScrollRect.gameObject.SetActive(false);
            sellListScrollRect.gameObject.SetActive(true);

            this.gameObject.SetActive(true);
            this.infoPanel.SetActive(false);

            tabBuy.interactable = true;
            tabSell.interactable = false;

            buyPanel.SetActive(false);
            sellPanel.SetActive(true);

            var progress = GameManager.Instance.Progress;

            foreach(var slot in sellListSlotPool) {
                slot.gameObject.SetActive(false);
            }

            Debug.Log(progress.Trades.Count);

            foreach(var pair in progress.Items) {
                int id = pair.Key;
                int count = pair.Value;

                var item = DataManager.Instance.Items.Find(e => e.Id == id);

                if(!item.CanSell)
                    continue;

                var listSlot = GetSellListSlot();

                listSlot.Init(item, count, sellListScrollRect, () => {
                    ShowInfo(item, false);
                });
            }

            foreach(var pair in progress.Monsters) {
                int id = pair.Key;
                int count = pair.Value;

                var listSlot = GetSellListSlot();

                var monster = DataManager.Instance.Monsters.Find(e => e.Id == id);

                listSlot.Init(monster, count, sellListScrollRect, () => {
                    ShowInfo(monster, false);
                });
            }

            this.textGold.text = progress.Gold.ToString("N0");

            OrderListSellSlots();

            Fit();
        }

        private TradeSellGridSlot GetSellGridSlot() {
            var slot = sellGridSlotPool.Where(e => !e.gameObject.activeInHierarchy).FirstOrDefault();

            if(slot == default(TradeSellGridSlot)) {
                slot = Instantiate(sellGridSlotPrefab, sellGridSlotContainer);
                sellGridSlotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            return slot;
        }

        private TradeSellListSlot GetSellListSlot() {
            var slot = sellListSlotPool.Where(e => !e.gameObject.activeInHierarchy).FirstOrDefault();

            if(slot == default(TradeSellListSlot)) {
                slot = Instantiate(sellListSlotPrefab, sellListSlotContainer);
                sellListSlotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            return slot;
        }

        private void OrderGridSellSlots() {
            var progress = GameManager.Instance.Progress;

            var gridSlots = sellGridSlotPool.Where(e => e.gameObject.activeInHierarchy).OrderByDescending(e => e.IsItem).ThenBy(e => e.Id).ToList();

            for(int i = 0; i < gridSlots.Count; i++) {
                gridSlots[i].transform.SetSiblingIndex(i);
            }
        }

        private void OrderListSellSlots() {
            var progress = GameManager.Instance.Progress;

            var listSlots = sellListSlotPool.Where(e => e.gameObject.activeInHierarchy).OrderByDescending(e => e.IsItem).ThenBy(e => e.Id).ToList();

            for(int i = 0; i < listSlots.Count; i++) {
                listSlots[i].transform.SetSiblingIndex(i);
            }
        }
        #endregion

        #region Info
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

                float discountRate = GameConsts.GetTradeDiscountRate(GameManager.Instance.Progress.Parameters[Parameter.Intelligence]);

                var price = Mathf.FloorToInt(sale.BuyPrice * (1 - discountRate));

                textPrice.text = price.ToString("N0");

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

                buttonConfirm.interactable = true;
            }

            Fit();
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

            this.unitStatus.SetUI(data.Status);
          
            this.textMonsterDescription.text = data.Status.GetAttackDescriptionString();
        }

        public void Confirm() {
            var progress = GameManager.Instance.Progress;

            if(this.buy) {
                if(data is ItemData) {
                    progress.BuyItem(data as ItemData);
                } else if(data is MonsterData) {
                    progress.BuyMonster(data as MonsterData);
                }

                OpenBuyPanel();
            } else {
                if(data is ItemData) {
                    progress.SellItem(data as ItemData);
                } else if(data is MonsterData) {
                    progress.SellMonster(data as MonsterData);
                }

                if(sellViewIsGridMode)
                    OpenGridSellPanel();
                else
                    OpenListSellPanel();

                if(data is ItemData) {
                    if(progress.Items.ContainsKey((data as ItemData).Id))
                        ShowInfo(data, false);
                } else if(data is MonsterData) {
                    if(progress.Monsters.ContainsKey((data as MonsterData).Id))
                        ShowInfo(data, false);
                }
            }
        }
        #endregion
    }
}
