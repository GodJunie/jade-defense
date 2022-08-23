using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "DailyRoutineData", menuName = "B409/Daily Routine Data")]
    public class DailyRoutineData : StageSequenceData {
        [HorizontalGroup("group", .5f)]
        [VerticalGroup("group/group")]
        [SerializeField]
        private List<string> scripts;

        [VerticalGroup("group/group")]
        [BoxGroup("group/group/Day")]
        [HideLabel]
        [SerializeField]
        private int day;

        [VerticalGroup("group/group")]
        [ListDrawerSettings(AddCopiesLastElement = true)]
        [SerializeField]
        private List<MonsterData> monsters;

        [HorizontalGroup("group/group/group", .5f)]
        [BoxGroup("group/group/group/Sales Count")]
        [HideLabel]
        [SerializeField]
        private int salesCount;

        [VerticalGroup("group/group")]
        [ListDrawerSettings()]
        [InfoBox("Total rate is zero. Please enter appearance rate", InfoMessageType.Error, "tradeAvailable")]
        [SerializeField]
        private List<TradeInfo> trades;

        [HorizontalGroup("group")]
        [VerticalGroup("group/group2")]
        [BoxGroup("group/group2/Enemies")]
        [SerializeField]
        private List<EnemyInfo> enemies;


        [VerticalGroup("group/group2")]
        [InlineEditor(Expanded = true)]
        [SerializeField]
        private BattleData battle;

#if UNITY_EDITOR
        private bool tradeAvailable {
            get {
                if(trades == null) return false;
                if(trades.Count == 0) return false;
                return trades.Sum(e => e.Rate) == 0;
            }
        }
        
        [HorizontalGroup("group/group/group")]
        [BoxGroup("group/group/group/Total Rate")]
        [HideLabel]
        [ShowInInspector]
        private float totalRate {
            get {
                return trades.Sum(e => e.Rate);
            }
        }

        [BoxGroup("group/group2/Enemies")]
        [Button("Load", Style = ButtonStyle.Box)]
        private void LoadEnemeis() {
            if(battle == null) return;

            this.enemies = battle.Enemies.Distinct().Select(e => new EnemyInfo(e, battle.Enemies.Count(e2 => e2 == e))).ToList();
        }
#endif

        public int Day => day;
        public List<string> Scripts => scripts;
        public BattleData Battle => battle;
        public int SalesCount => salesCount;
        public List<TradeInfo> Trades => trades;
        public List<MonsterData> Monsters => monsters;

        [Serializable]
        public class TradeInfo {
            [HorizontalGroup("group")]
            [BoxGroup("group/Sale")]
            [HideLabel]
            [SerializeField]
            [InfoBox("This Object is not on sale!", InfoMessageType.Error, "canNotSale")]
            private ScriptableObject sale;
            [HorizontalGroup("group", 100f)]
            [BoxGroup("group/Rate")]
            [HideLabel]
            [SerializeField]
            private float rate;

#if UNITY_EDITOR

            [HorizontalGroup("group", 100f)]
            [VerticalGroup("group/group")]
            [BoxGroup("group/group/Buy")]
            [HideLabel]
            [ShowInInspector]
            private int buy {
                get {
                    if(sale == null) return 0;
                    if(!(sale is ISale)) return 0;
                    return (sale as ISale).BuyPrice;
                }
            }
   
            [BoxGroup("group/group/Sell")]
            [HideLabel]
            [ShowInInspector]
            private int sell {
                get {
                    if(sale == null) return 0;
                    if(!(sale is ISale)) return 0;
                    return (sale as ISale).SellPrice;
                }
            }

            private bool canNotSale {
                get {
                    if(sale == null) return true;
                    return !(sale is ISale);
                }
            }
#endif

            public ScriptableObject Sale => sale;
            public float Rate => rate;

            public IEnumerable<Type> GetFilteredTypeList() {
                return new List<Type>() { typeof(MonsterData), typeof(ItemData) };
            }
        }
  
        [Serializable]
        public class EnemyInfo {
            [HorizontalGroup("group", .5f)]
            [BoxGroup("group/Enemy")]
            [HideLabel]
            [SerializeField]
            private UnitData enemy;
            [HorizontalGroup("group", .5f)]
            [BoxGroup("group/Count")]
            [HideLabel]
            [SerializeField]
            private int count;

            public UnitData Enemy => enemy;
            public int Count => count;

            public EnemyInfo(UnitData enemy, int count) {
                this.enemy = enemy;
                this.count = count;
            }
        }
    }
}