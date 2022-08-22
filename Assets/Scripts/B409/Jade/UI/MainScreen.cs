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
using Spine.Unity;


namespace B409.Jade.UI {
    using Game;
    using Data;

    public class MainScreen : MonoBehaviour {
        #region Serialized Members
        [SerializeField]
        private TMP_Text textDDay;
        [SerializeField]
        private TMP_Text textCurrentAP;
        [SerializeField]
        private TMP_Text textMaxAP;

        [SerializeField]
        private TMP_Text textParameters;

        [SerializeField]
        private TMP_Text textScript;
        [SerializeField]
        private float textSpeed = 20f;
        
        [SerializeField]
        private Image imageFade;
        [SerializeField]
        private SkeletonGraphic jadeAnim;
        [SerializeField]
        private string jadeIdle;
        [SerializeField]
        private string jadeInteract;

        public DailyRoutineData data;
        #endregion

        #region Mono
        private void Start() {
            imageFade.gameObject.SetActive(true);
            imageFade.color = Color.white;
            imageFade.DOFade(0f, 1f);

            GameManager.Instance.TurnStart();

            var data = GameManager.Instance.CurrentStageSequence;

            if(!(data is DailyRoutineData)) {
                Debug.LogError(string.Format("Stage Sequence is not DailyRoutineData!"));
            }

            this.data = data as DailyRoutineData;

            this.jadeAnim.AnimationState.Complete += (entry) => {
                if(entry.Animation.Name == jadeInteract) {
                    jadeAnim.AnimationState.SetAnimation(0, jadeIdle, true);
                }
            };

              
            this.Log("");
            this.Hint();
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

        public void Hint() {
            jadeAnim.AnimationState.SetAnimation(0, jadeInteract, false);

            if(this.data.Scripts.Count > 0) {
                var hint = this.data.Scripts[UnityEngine.Random.Range(0, data.Scripts.Count)];
                this.Log(hint);
            }
        }

        public void Log(string text) {
            this.textScript.DOKill();
            this.textScript.text = "";
            this.textScript.DOText(text, textSpeed).SetSpeedBased();
        }

        #region UI
        public void Init() {
            var progress = GameManager.Instance.Progress;
           
            this.textDDay.text = string.Format("D - {0}", progress.DDay);

            float strength = progress.Parameters[Parameter.Strength];
            float deft = progress.Parameters[Parameter.Deft];
            float endurance = progress.Parameters[Parameter.Endurance];
            float intelligence = progress.Parameters[Parameter.Intelligence];
            float luck = progress.Parameters[Parameter.Luck];

            this.textParameters.text = string.Format("{0:0}\n{1:0}\n{2:0}\n{3:0}\n{4:0}", strength, deft, endurance, intelligence, luck);

            this.textCurrentAP.text = progress.AP.ToString("0");
            this.textMaxAP.text = progress.MaxAP.ToString("0");
        }
        #endregion
    }
}
