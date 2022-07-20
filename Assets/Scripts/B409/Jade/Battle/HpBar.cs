using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.Battle {
    using Data;


    public class HpBar : MonoBehaviour {
        [SerializeField]
        private Image imageFill;
        [SerializeField]
        private Sprite fillBlue;
        [SerializeField]
        private Sprite fillRed;
        [SerializeField]
        private Text textHp;

        private float maxHp;
        private float hp;

        public void Init(UnitData Data, bool isPlayer) {
            if(isPlayer) {
                this.imageFill.sprite = fillBlue;
            } else {
                this.imageFill.sprite = fillRed;
            }

            this.hp = this.maxHp = Data.Status.Hp;
            this.textHp.text = string.Format("{0:0}/{1:0}", this.hp, this.maxHp);
        }

        public void SetHp(float hp) {
            this.hp = hp;
            this.textHp.text = string.Format("{0:0}/{1:0}", this.hp, this.maxHp);
        }
    }
}