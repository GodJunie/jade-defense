using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    public class GameProgress {
        public int DDay { get; private set; }
        public int Stage { get; private set; }
        public float AP { get; private set; }
        public Dictionary<int, int> Items { get; private set; }
        public Dictionary<Parameter, float> Parameters { get; private set; }

        public GameProgress() {
            this.Items = new Dictionary<int, int>();
            this.Parameters = new Dictionary<Parameter, float>();
        }

        public static GameProgress FromJson(string json) {
            var progress = new GameProgress();
            return progress;
        }

        public void AddItem(int id, int count) {
            if(this.Items.ContainsKey(id)) {
                this.Items[id] += count;
            } else {
                this.Items[id] = count;
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
    }
}