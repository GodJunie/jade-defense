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
        [FoldoutGroup("Info/Colors")]
        [ColorUsage(true)]
        [SerializeField]
        private Color hpColor = Color.white;
        [FoldoutGroup("Info/Colors")]
        [ColorUsage(true)]
        [SerializeField]
        private Color damageColor = Color.white;
        [FoldoutGroup("Info/Colors")]
        [ColorUsage(true)]
        [SerializeField]
        private Color slowColor = Color.white;
        [FoldoutGroup("Info/Colors")]
        [ColorUsage(true)]
        [SerializeField]
        private Color durationColor = Color.white;
        [FoldoutGroup("Info/Colors")]
        [ColorUsage(true)]
        [SerializeField]
        private Color targetColor = Color.white;


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

            this.textAttackDescription.text = GetAttackDescription(data.Status);

            panelInfo.SetActive(true);
        }

        private string GetAttackDescription(Status status) {
            string d = "";

            if(status.AttackMode == AttackMode.Heal) {
                d = string.Format("Heal <sprite name=Hp> <color={0}>{1} HP</color> ", hpColor.GetHexString(), status.Heal);
            } else {
                d = string.Format("Inflicts <sprite name=Atk> <color={0}>{1} dmg</color> ", damageColor.GetHexString(), status.Atk);
            }

            if(status.TargetCount == 0) {
                d += string.Format("to the <sprite index=7> <color={1}>all {0}</color> within range ", status.TargetEnemy ? "opponents" : "allies", targetColor.GetHexString());
            } else {
                string t = string.Format("<sprite index=7> <color={2}>{0} {1}</color>", status.TargetCount, status.TargetEnemy ? "opponents" : "allies", targetColor.GetHexString());
                switch(status.TargetFilterMode) {
                case TargetFilterMode.Hp:
                    d += string.Format("to {0} with {1} current HP first ", t, status.Descending ? "high" : "low");
                    break;
                case TargetFilterMode.MaxHp:
                    d += string.Format("to {0} with {1} maximum HP first ", t, status.Descending ? "high" : "low");
                    break;
                case TargetFilterMode.Distance:
                    d += string.Format("to the {1} {0} ", t, status.Descending ? "farthest" : "nearest");
                    break;
                case TargetFilterMode.Index:
                    d += string.Format("to {0} ", t);
                    break;
                default:
                    break;
                }
            }

            switch(status.AttackMode) {
            case AttackMode.Attack:
                break;
            case AttackMode.DamageOverTime:
                d += string.Format("over {0:0.#} secs", status.Duration);
                break;
            case AttackMode.Heal:
                break;
            case AttackMode.Stun:
                d += string.Format("and stuns for {0:0.#} secs", status.Duration);
                break;
            case AttackMode.Slow:
                d += string.Format("and slow down <sprite name=Slow> <color={2}>{0:P1}</color> speed for <color={3}>{1:0.#} secs</color>", status.SlowRate, status.Duration, slowColor.GetHexString(), durationColor.GetHexString());
                break;
            default:
                break;
            }

            d += ".";
            return d;
        }

        public void Buy() {
            GameManager.Instance.BuyMonster(this.data);
            this.OpenBuyPanel();
            this.ShowInfo(this.data, true);
        }
    }
}