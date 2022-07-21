using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    public class DamageOverTime {
        public float Timer { get; private set; }
        public float Interval { get; private set; }
        public float Duration { get; private set; }
        public float Damage { get; private set; }

        public DamageOverTime(float damage, float duration, float interval) {
            this.Damage = damage;
            this.Timer = 0f;
            this.Duration = duration;
            this.Interval = interval;
        }

        public bool Tick() {
            Timer -= Time.deltaTime;
            Duration -= Time.deltaTime;
            if(Timer < 0) {
                Timer += Interval;
                return true;
            } else {
                return false;
            }
        }
    }
}
