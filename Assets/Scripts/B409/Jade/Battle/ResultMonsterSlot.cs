using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace B409.Jade.Battle {
    public class ResultMonsterSlot : MonoBehaviour {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private GameObject checkmark;
            
        public void Init(Sprite sprite, bool die) {
            icon.sprite = sprite;
            checkmark.SetActive(die);
            icon.color = die ? Color.gray : Color.white;
        }
    }
}