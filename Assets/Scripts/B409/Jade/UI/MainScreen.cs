// System
using System.Collections;
using System.Collections.Generic;
// UnityEngine
using UnityEngine;
using UnityEngine.UI;
// Etc;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;


namespace B409.Jade.UI {
    using Game;
    using Data;

    public class MainScreen : MonoBehaviour {
        #region Serialized Members
        [SerializeField]
        private TMP_Text textDDay;
        [SerializeField]
        private Image imageAPFill;


        [SerializeField]
        private TMP_Text textDft;
        [SerializeField]
        private TMP_Text textStr;
        [SerializeField]
        private TMP_Text textInt;
        [SerializeField]
        private TMP_Text textLuk;
        [SerializeField]
        private TMP_Text textEdr;


        [SerializeField]
        private Image imageBackground;

        [SerializeField]
        private Image imageFade;
        #endregion

        #region Mono
        private void Start() {
            imageFade.gameObject.SetActive(true);
            imageFade.color = Color.white;
            imageFade.DOFade(0f, 1f);
            GameManager.Instance.TurnStart();
            this.Init();
        }
        #endregion

        #region Action
        public async void TurnEnd() {
            // AP 남아있으면 AP 확인 하라고 하고
            // 다음날이 전투날인데 몬스터 부족하면 몬스터 구매 하라고 하고
            // 거래상이 있는데 재고 많으면 안살거냐고 물어보고
            // ㅇㅋ 확정
            // Fade 시키고
            await imageFade.DOFade(1f, 0.5f);
            GameManager.Instance.TurnEnd();
        }
        #endregion

        #region UI
        public void Init() {
            var progress = GameManager.Instance.Progress;

            var stage = DataManager.Instance.Stages[progress.Stage];
            var stageSequence = stage.Datas[progress.StageSequence];
            if(!(stageSequence is DailyRoutineData)) {
                Debug.LogError(string.Format("Stage Sequence is not DailyRoutineData!"));
            }

            this.textDDay.text = string.Format("D - {0}", progress.DDay);

            this.textDft.text = string.Format("dft {0}", progress.Parameters[Parameter.Deft]);
            this.textStr.text = string.Format("str {0}", progress.Parameters[Parameter.Strength]);
            this.textInt.text = string.Format("int {0}", progress.Parameters[Parameter.Intelligence]);
            this.textLuk.text = string.Format("luk {0}", progress.Parameters[Parameter.Luck]);
            this.textEdr.text = string.Format("edr {0}", progress.Parameters[Parameter.Endurance]);

            this.imageAPFill.fillAmount = progress.AP / progress.MaxAP;
        }
        #endregion
    }
}
