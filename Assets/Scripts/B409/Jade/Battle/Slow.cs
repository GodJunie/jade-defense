using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    public class Slow  {
        public float Duration { get; private set; }
        public float SlowRate { get; private set; }

        public Slow(float duration, float slowRate) {
            this.Duration = duration;
            this.SlowRate = slowRate;
        }

        public void Tick() {
            this.Duration -= Time.deltaTime;
        }
    }
}