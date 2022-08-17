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
        private TMP_Text textParameters;
        
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
            // AP ���������� AP Ȯ�� �϶�� �ϰ�
            // �������� �������ε� ���� �����ϸ� ���� ���� �϶�� �ϰ�
            // �ŷ����� �ִµ� ��� ������ �Ȼ�ųİ� �����
            // ���� Ȯ��
            // Fade ��Ű��
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

            float strength = progress.Parameters[Parameter.Strength];
            float deft = progress.Parameters[Parameter.Deft];
            float endurance = progress.Parameters[Parameter.Endurance];
            float intelligence = progress.Parameters[Parameter.Intelligence];
            float luck = progress.Parameters[Parameter.Luck];

            this.textParameters.text = string.Format("{0:0}\n{1:0}\n{2:0}\n{3:0}\n{4:0}", strength, deft, endurance, intelligence, luck);

            this.imageAPFill.fillAmount = progress.AP / progress.MaxAP;
        }
        #endregion
    }
}
