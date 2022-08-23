using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    public class DamageOverTime {
        public float Timer { get; private set; }
        public float Interval { get; private set; }
        public float Damage { get; private set; }
        public int Count { get; private set; }

        public DamageOverTime(float damage, float duration, int count) {
            this.Damage = damage;
            this.Timer = 0f;
            this.Count = count;
            this.Interval = duration / (count - 1);
        }

        public bool Tick() {
            Timer -= Time.deltaTime;
            if(Timer < 0) {
                Timer += Interval;
                Count--;
                return true;
            } else {
                return false;
            }
        }
    }
}
