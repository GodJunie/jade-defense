using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    public class SettingsPanel : MonoBehaviour {
        [SerializeField]
        private Slider bgmSlider;
        [SerializeField]
        private Slider sfxSlider;

        public void Open() {
            this.gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}