using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    using Data;

    public class GameProgress {
        public int DDay { get; private set; }
        public int Stage { get; private set; }
        public float AP { get; private set; }
        public Dictionary<int, int> Items { get; private set; }
        public Dictionary<Parameter, float> Parameters { get; private set; }

        public GameProgress() {
            this.Items = new Dictionary<int, int>();
            this.Parameters = new Dictionary<Parameter, float>() {
                { Parameter.Deft, 0 },
                { Parameter.Strength, 0 },
                { Parameter.Intelligence, 0 },
                { Parameter.Luck, 0 },
                { Parameter.Endurance, 0 },
            };
        }

        public static GameProgress FromJson(string json) {
            var progress = new GameProgress();
            return progress;
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