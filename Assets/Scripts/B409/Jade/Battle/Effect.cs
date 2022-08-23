using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    public class Effect : MonoBehaviour {
        [SerializeField]
        private float duration;

        private float timer = 0f;

        private void Update() {
            timer -= Time.deltaTime;
            if(timer < 0f) {
                this.gameObject.SetActive(false);
            }
        }

        public void EffectOn() {
            this.timer = duration;
            this.gameObject.SetActive(true);
        }
    }
}