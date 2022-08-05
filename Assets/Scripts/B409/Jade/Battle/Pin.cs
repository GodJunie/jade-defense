using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.Battle {
    public class Pin : MonoBehaviour {
        [SerializeField]
        [ColorUsage(true)]
        private Color alliesColor = Color.blue;
        [SerializeField]
        [ColorUsage(true)]
        private Color opponentsColor = Color.red;

        private new RectTransform transform;
        private Transform target;
        private float ratio;

        private void Awake() {
            this.transform = GetComponent<RectTransform>();
        }

        public void Init(UnitController target, float ratio, bool isPlayer) {
            this.target = target.transform;
            target.OnDead += () => {
                Destroy(gameObject);
            };
            this.ratio = ratio;

            this.transform.anchoredPosition = new Vector2(this.target.position.x * ratio, 0f);
            this.gameObject.SetActive(true);
            GetComponent<Image>().color = isPlayer ? alliesColor : opponentsColor;
        }

        // Update is called once per frame
        void Update() {
            this.transform.anchoredPosition = new Vector2(target.position.x * ratio, 0f);
        }
    }
}