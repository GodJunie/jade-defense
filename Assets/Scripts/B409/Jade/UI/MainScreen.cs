// System
using System.Collections;
using System.Collections.Generic;
// UnityEngine
using UnityEngine;
using UnityEngine.UI;
// Editor;
using Sirenix.OdinInspector;


namespace B409.Jade.UI {
    using Game;

    public class MainScreen : MonoBehaviour {
        #region Serialized Members
        [SerializeField]
        private Text textDDay;
        [SerializeField]
        private Image imageAPFill;
        #endregion

        #region UI
        private void Init() {
            var progress = GameManager.Instance.Progress;

            this.textDDay.text = string.Format("D-{0}", progress.DDay);
        }
        #endregion
    }
}
