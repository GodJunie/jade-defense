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


        [SerializeField]
        private Text textDft;
        [SerializeField]
        private Text textStr;
        [SerializeField]
        private Text textInt;
        [SerializeField]
        private Text textLuk;
        [SerializeField]
        private Text textEdr;
        #endregion

        #region Mono
        private void Start() {
            this.Init();
        }
        #endregion

        #region UI

        private void Init() {
            var progress = GameManager.Instance.Progress;

            this.textDDay.text = string.Format("D-{0}", progress.DDay);

            this.textDft.text = string.Format("DFT {0}", progress.Parameters[Parameter.Deft]);
            this.textStr.text = string.Format("STR {0}", progress.Parameters[Parameter.Strength]);
            this.textInt.text = string.Format("INT {0}", progress.Parameters[Parameter.Intelligence]);
            this.textLuk.text = string.Format("LUK {0}", progress.Parameters[Parameter.Luck]);
            this.textEdr.text = string.Format("EDR {0}", progress.Parameters[Parameter.Endurance]);
        }
        #endregion
    }
}
