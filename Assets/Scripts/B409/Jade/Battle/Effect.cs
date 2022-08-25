using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    using Game;

    public class Effect : MonoBehaviour {
        [SerializeField]
        private AudioClip sfx;

        private float timer = 0f;


        private void Update() {
            timer -= Time.deltaTime;
            if(timer < 0f) {
                this.gameObject.SetActive(false);
            }
        }
     
        public void EffectOn(float duration) {
            this.timer = duration;
            this.gameObject.SetActive(true);
            SoundManager.Instance.PlaySfx(sfx);
        }
    }
}