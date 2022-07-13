using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Battle {
    using Data;

    public class BattleController : MonoBehaviour {
        [SerializeField]
        private float costMax;
        [SerializeField]
        private float cost;
        [SerializeField]
        private float costRecovery;

        private BattleData data;

        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            cost = Mathf.Clamp(cost + costRecovery * Time.deltaTime, 0f, costMax);
        }
    }
}