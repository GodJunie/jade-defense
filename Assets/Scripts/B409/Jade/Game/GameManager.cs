using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    using Data;

    public class GameManager : SingletonBehaviour<GameManager> {
        #region Members 
        public GameProgress Progress { get; private set; }
        #endregion

        #region Mono
        protected override void Awake() {
            base.Awake();
            Progress = new GameProgress();
        }
        #endregion

        #region Farming
        public void Farming(FarmingLevelTable.LevelData data) {
            int count = 5;
            var items = new Dictionary<int, int>();

            for(int i = 0; i < count; i++) {
                Debug.Log("get");

                var sum = data.Datas.Sum(e => e.Rate);
                var rand = UnityEngine.Random.Range(0f, sum);

                foreach(var rateData in data.Datas) {
                    if(rand < rateData.Rate) {
                        var item = rateData.Item;
                        if(items.ContainsKey(item.Id))
                            items[item.Id] += 1;
                        else
                            items.Add(item.Id, 1);
                        break;
                    }

                    rand -= rateData.Rate;
                }
            }
         
            Progress.AddItems(items);
        }
        #endregion
    }
}