using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;
    using Battle;

    public class EnemyPanel : MonoBehaviour {
        [SerializeField]
        private Transform slotContainer;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private EnemySlot slotPrefab;

        [SerializeField]
        private Image imageIcon;
        [SerializeField]
        private TMP_Text textName;
        [SerializeField]
        private UnitStatus unitStatus;
        [SerializeField]
        private TMP_Text textDescription;

        [SerializeField]
        private GameObject infoPanel;

        private List<EnemySlot> slotPool;

        public void Open() {
            var data = GameManager.Instance.CurrentStageSequence as DailyRoutineData;

            infoPanel.SetActive(false);

            foreach(var slot in slotPool) {
                slot.gameObject.SetActive(false);
            }

            foreach(var enemy in data.Enemies) {
                var enemyData  = enemy.Enemy;
                var count = enemy.Count;

                var slot = GetEnemySlot();

                slot.Init(enemyData, count, scrollRect, () => {
                    ShowInfo(enemyData);
                });
            }
        }

        private EnemySlot GetEnemySlot() {
            var slot = slotPool.Where(e => !e.gameObject.activeInHierarchy).FirstOrDefault();

            if(slot == default(EnemySlot)) {
                slot = Instantiate(slotPrefab, slotContainer);
                slotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            return slot;
        }

        private void ShowInfo(UnitData data) {
            this.imageIcon.sprite = data.Icon;
            this.textName.text = data.Name;
            this.unitStatus.SetUI(data.Status);
            this.textDescription.text = data.Status.GetAttackDescriptionString();

            infoPanel.SetActive(true);
        }
    }
}