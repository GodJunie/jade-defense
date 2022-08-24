using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.UI {
    using Game;
    using Data;

    public class SplashScreen : MonoBehaviour {
        private void Start() {
            GameManager.Instance.LoadTitleScene();
        }
    }
}