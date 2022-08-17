using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// Json
using Newtonsoft.Json;

namespace B409.Jade.Game {
    using Data;

    public class GameProgress {
        public int DDay { get; private set; }
        public int Stage { get; private set; }
        public int StageSequence { get; private set; }
        public float AP { get; private set; }
        public Dictionary<int, int> Items { get; private set; }
        public Dictionary<int, int> Monsters { get; private set; }
        public Dictionary<Parameter, float> Parameters { get; private set; }
        public Dictionary<int, int> Trades { get; private set; }
        public int RefreshCount { get; private set; }

        public int Gold {
            get {
                if(Items.ContainsKey(GameConsts.GOLD_ID)) {
                    return Items[GameConsts.GOLD_ID];
                } else {
                    return 0;
                }
            }
        }

        public float MaxAP {
            get {
                return Mathf.Lerp(100f, 200f, Parameters[Parameter.Endurance] / 120f);
            }
        }

        [JsonConstructor]
        public GameProgress(int dDay, int stage, int stageSequence, float ap, Dictionary<int, int> items, Dictionary<int, int> monsters, Dictionary<Parameter, float> parameters, Dictionary<int, int> trades, int refreshCount) {
            this.DDay = dDay;
            this.Stage = stage;
            this.StageSequence = stageSequence;
            this.AP = ap;
            this.Items = items;
            this.Monsters = monsters;
            this.Parameters = parameters;
            this.Trades = trades;
            this.RefreshCount = refreshCount;
        }

        public GameProgress() {
            this.Items = new Dictionary<int, int>();
            this.Monsters = new Dictionary<int, int>();
            this.Parameters = new Dictionary<Parameter, float>() {
                { Parameter.Deft, 0 },
                { Parameter.Strength, 0 },
                { Parameter.Intelligence, 0 },
                { Parameter.Luck, 0 },
                { Parameter.Endurance, 0 },
            };
            this.Trades = new Dictionary<int, int>();
        }

        public static GameProgress FromJson(string json) {
            return JsonConvert.DeserializeObject<GameProgress>(json);
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }

        public void DailyRoutineStart(DailyRoutineData data) {
            this.DDay = data.Day;
            this.RefreshCount = 1;
            InitTrades(data);
        }

        public void RefreshTrades(DailyRoutineData data) {
            this.RefreshCount--;
            InitTrades(data);
        }

        public void Trade(int id) {
            if(this.Trades.ContainsKey(id)) {
                Trades[id]--;
            } else {
                throw new Exception(string.Format("there is no id in trades, id: {0}", id));
            }
        }

        private void InitTrades(DailyRoutineData data) {
            this.Trades.Clear();
            float sum = data.Trades.Sum(e => e.Rate);
            for(int i = 0; i < data.SalesCount; i++) {
                float rand = UnityEngine.Random.Range(0f, sum);
                foreach(var trade in data.Trades) {
                    if(rand < trade.Rate) {
                        if(trade.Sale != null && trade.Sale is ISale) {
                            int id = (trade.Sale as IDataID).Id;

                            if(this.Trades.ContainsKey(id)) {
                                this.Trades[id]++;
                            } else {
                                this.Trades.Add(id, 1);
                            }
                        }
                        break;
                    } else {
                        rand -= trade.Rate;
                    }
                }
            }
        }

        public void TurnStart() {
            this.AP = MaxAP;
        }

        public void TurnEnd() {
            this.DDay--;
        }

        public void NextStage() {
            this.Stage++;
            this.StageSequence = 0;
        }

        public void NextStageSequence() {
            this.StageSequence++;
        }

        public void ConsumeAP(float ap) {
            this.AP -= ap;
        }

        public void GetParameters(List<ParameterValue> parameters) {
            foreach(var param in parameters) {
                this.Parameters[param.Parameter] += param.Value;
            }
        }

        public int GetItemCount(int id) {
            if(this.Items.ContainsKey(id))
                return this.Items[id];
            return 0;
        }

        public void AddItem(int id, int count) {
            Debug.Log(string.Format("Add Item, id: {0}, count: {1}", id, count));
            if(this.Items.ContainsKey(id)) {
                this.Items[id] += count;
            } else {
                this.Items.Add(id, count);
            }
        }

        public void AddItems(Dictionary<int, int> items) {
            foreach(var item in items) {
                this.AddItem(item.Key, item.Value);
            }
        }

        public void UseItem(int id, int count) {
            if(!this.Items.ContainsKey(id)) {
                throw new Exception(string.Format("Item doesn't exist, id : {0}", id));
            }

            if(this.Items[id] < count) {
                throw new Exception(string.Format("Not enough item, id : {0}, count : {1}, remain : {2}", id, count, this.Items[id]));
            }

            this.Items[id] -= count;
            if(this.Items[id] == 0)
                this.Items.Remove(id);
        }

        public void UseItems(Dictionary<int, int> items) {
            foreach(var item in items) {
                this.UseItem(item.Key, item.Value);
            }
        }

        public void UseItems(List<MaterialData> materials) {
            foreach(var mat in materials) {
                this.UseItem(mat.Item.Id, mat.Count);
            }
        }

        public bool CheckItemEnough(int id, int count) {
            return GetItemCount(id) >= count;
        }

        public bool CheckItemsEnough(Dictionary<int, int> items) {
            foreach(var pair in items) {
                if(!CheckItemEnough(pair.Key, pair.Value))
                    return false;
            }
            return true;
        }

        public bool CheckItemsEnough(List<MaterialData> materials) {
            foreach(var mat in materials) {
                if(!CheckItemEnough(mat.Item.Id, mat.Count))
                    return false;
            }
            return true;
        }

        public void AddMonster(int id, int count) {
            Debug.Log(string.Format("Add Monster, id: {0}, count: {1}", id, count));
            if(this.Monsters.ContainsKey(id)) {
                this.Monsters[id] += count;
            } else {
                this.Monsters.Add(id, count);
            }
        }

        public void UseMonster(int id, int count) {
            if(!this.Monsters.ContainsKey(id)) {
                throw new Exception(string.Format("Monster doesn't exist, id : {0}", id));
            }

            if(this.Monsters[id] < count) {
                throw new Exception(string.Format("Not enough monster, id : {0}, count : {1}, remain : {2}", id, count, this.Monsters[id]));
            }

            this.Monsters[id] -= count;
            if(this.Monsters[id] == 0)
                this.Monsters.Remove(id);
        }

        public bool CheckParameterValidation(List<ParameterValue> parameters) {
            foreach(var p in parameters) {
                if(Parameters[p.Parameter] < p.Value)
                    return false;
            }
            return true;
        }

        public string GetInquiredParametersText(List<ParameterValue> parameters) {
            string text = "";
            foreach(var p in parameters) {
                text += string.Format("<color={0}>{1} {2} </color>", p.Value > this.Parameters[p.Parameter] ? "red" : "white", p.Parameter.ToCompatString(), p.Value.ToString("N0"));
            }
            return text;
        }
    }
}