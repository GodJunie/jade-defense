using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

namespace B409.Jade.UI {

    public class DialogueCharacter : MonoBehaviour {
        [SerializeField]
        private List<SkeletonGraphic> skeletons;
        [SerializeField]
        private bool isLeft;
        [SerializeField]
        private Color disabledColor = new Color32(50, 50, 50, 255);
        [SerializeField]
        private Color highlightedColor = Color.white;

        [SerializeField]
        [ValueDropdown("animations")]
        private string appearAimation;
        [SerializeField]
        [ValueDropdown("animations")]
        private string idleAimation = "Idle";

        private List<Graphic> graphics;

        private void Awake() {
            graphics = GetComponentsInChildren<Graphic>().ToList();
        }

        public void Appear(bool isLeft) {
            this.gameObject.SetActive(true);

            if(this.isLeft != isLeft) {
                transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            } else {
                transform.localRotation = Quaternion.identity;
            }

            foreach(var skeleton in skeletons) {
                skeleton.AnimationState.SetAnimation(0, appearAimation, false);
                skeleton.AnimationState.AddAnimation(0, idleAimation, true, 0f);
            }
        }

        public void SetFocus(bool focus, float duration = 0.5f) {
            foreach(var graphic in graphics) {
                FadeColor(graphic, focus, duration);
            }
        }

        private async void FadeColor(Graphic graphic, bool focus, float duration) {
            Color originColor = graphic.color;
            Color color = focus ? highlightedColor : disabledColor;

            for(float i = 0; i < duration; i += Time.fixedDeltaTime) {
                await UniTask.WaitForFixedUpdate();
                graphic.color = originColor.Lerp(color, i / duration);
            }
        }

#if UNITY_EDITOR
        public string[] animations {
            get {
                return this.skeletons[0].Skeleton.Data.Animations.Select(e => e.Name).ToArray();
            }
        }
#endif
    }
}