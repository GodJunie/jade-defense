using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;

namespace B409.Jade.UI {
    using Game;
    using Battle;
    using Data;

    public class PartyPanel : MonoBehaviour {
        [SerializeField]
        private BattleController battleController;

        [TitleGroup("Party")]
        [SerializeField]
        private Transform partySlotContainer;
        [TitleGroup("Party")]
        [SerializeField]
        private ScrollRect partySlotScrollRect;
        [TitleGroup("Party")]
        [SerializeField]
        private PartyMonsterSlot partySlotPrefab;

        [TitleGroup("Party")]
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private MonsterOwnedSlot monsterSlotPrefab;
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private Transform monsterSlotContainer;
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private ScrollRect monsterScrollRect;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject panelInfo;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private Image imageSelectedMonster;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterName;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private UnitStatus selectedUnitStatus;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterDescription;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject buttonMonsterIn;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject buttonMonsterOut;

        private MonsterData selectedMonsterData;
        private Dictionary<int, MonsterOwnedSlot> monsterSlotPool = new Dictionary<int, MonsterOwnedSlot>();
        private List<PartyMonsterSlot> partySlots = new List<PartyMonsterSlot>();
        private PartyMonsterSlot selectedPartySlot;

        public void Open() {
            gameObject.SetActive(true);
            panelInfo.SetActive(false);

            var progress = GameManager.Instance.Progress;

            foreach(var monster in progress.Monsters) {
                var slot = Instantiate(monsterSlotPrefab, monsterSlotContainer);
                int id = monster.Key;
                int count = monster.Value;
                var data = DataManager.Instance.Monsters.Find(e => e.Id == id);
                slot.Init(data, count, monsterScrollRect, () => {
                    ShowMonsterInfo(data, true);
                });
                this.monsterSlotPool.Add(id, slot);
            }
        }

        private void ShowMonsterInfo(MonsterData data, bool monsterIn) {
            this.selectedMonsterData = data;

            panelInfo.SetActive(true);

            imageSelectedMonster.sprite = data.Icon;
            textSelectedMonsterName.text = data.Name;
            // status
            selectedUnitStatus.SetUI(data.Status);
            textSelectedMonsterDescription.text = data.Status.GetAttackDescriptionString();

            buttonMonsterIn.SetActive(monsterIn);
            buttonMonsterOut.SetActive(!monsterIn);
        }

        public void MonsterInParty(bool monsterIn) {
            if(monsterIn) {
                int id = selectedMonsterData.Id;

                var progress = GameManager.Instance.Progress;
                progress.UseMonster(id, 1);

                if(progress.Monsters.ContainsKey(id)) {
                    this.monsterSlotPool[id].Init(selectedMonsterData.Icon, progress.Monsters[id]);
                } else {
                    this.monsterSlotPool[id].gameObject.SetActive(false);
                }

                var slot = Instantiate(partySlotPrefab, partySlotContainer);

                slot.Init(selectedMonsterData, partySlotScrollRect, () => {
                    ShowMonsterInfo(slot.Data, false);
                    this.selectedPartySlot = slot;
                }, () => {
                    var index = partySlots.IndexOf(slot);

                    if(index > 0) {
                        var prev = partySlots[index - 1];
                        partySlots[index - 1] = slot;
                        partySlots[index] = prev;
                        SetPartySlotIndex();
                    }
                }, () => {
                    var index = partySlots.IndexOf(slot);

                    if(index < partySlots.Count - 1) {
                        var next = partySlots[index + 1];
                        partySlots[index + 1] = slot;
                        partySlots[index] = next;
                        SetPartySlotIndex();
                    }
                });

                slot.SetIndex(partySlots.Count);
                partySlots.Add(slot);

                selectedMonsterData = null;
            } else {
                partySlots.Remove(selectedPartySlot);
                var monsterData = selectedPartySlot.Data;
                Destroy(selectedPartySlot.gameObject);

                SetPartySlotIndex();
                var progress = GameManager.Instance.Progress;
                progress.AddMonster(monsterData.Id, 1);

                var slot = monsterSlotPool[monsterData.Id];
                slot.gameObject.SetActive(true);
                slot.Init(monsterData.Icon, progress.Monsters[monsterData.Id]);

                selectedPartySlot = null;
            }

            panelInfo.SetActive(false);
        }

        private void SetPartySlotIndex() {
            for(int i = 0; i < partySlots.Count; i++) {
                partySlots[i].SetIndex(i);
            }
        }

        private bool gameStart = false;

        public async void GameStart() {
            if(this.partySlots.Count == 0)
                return;
            if(gameStart)
                return;

            gameStart = true;

            battleController.InitMonsterData(this.partySlots.Select(e => e.Data as UnitData).ToList());

            var tasks = new List<UniTask>();
            foreach(var graphic in this.GetComponentsInChildren<Graphic>()) {
                tasks.Add(graphic.DOFade(0f, 1f).ToUniTask());
            }
            tasks.Add(this.transform.DOScale(2f, 1f).ToUniTask());
            await UniTask.WhenAll(tasks);

            gameObject.SetActive(false);
        }
    }
}