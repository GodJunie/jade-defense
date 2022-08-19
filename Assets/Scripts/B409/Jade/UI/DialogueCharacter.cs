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

        private bool focus;

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

            this.focus = false;
        }

        public async UniTask SetFocus(bool focus, float duration = 0.5f) {
            if(this.focus == focus) return;

            this.focus = focus;

            Color originColor = focus ? disabledColor : highlightedColor;
            Color color = focus ? highlightedColor : disabledColor;

            var tasks = new List<UniTask>();

            foreach(var graphic in graphics) {
                tasks.Add(FadeColor(graphic, originColor, color, duration));
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask FadeColor(Graphic graphic, Color originColor, Color color, float duration) {
            graphic.color = originColor;
            for(float i = 0; i < duration; i += Time.fixedDeltaTime) {
                await UniTask.WaitForFixedUpdate();
                graphic.color = originColor.Lerp(color, i / duration);
            }
            graphic.color = color;
        }

        public async UniTask FadeOut(float duration = 0.5f) {
            Color color = Color.black;
            color.a = 0f;

            var tasks = new List<UniTask>();

            foreach(var graphic in graphics) {
                tasks.Add(FadeColor(graphic, graphic.color, color, duration));
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask FadeIn(float duration = 0.5f) {
            Color color = this.focus ? highlightedColor : disabledColor;

            var tasks = new List<UniTask>();

            foreach(var graphic in graphics) {
                tasks.Add(FadeColor(graphic, graphic.color, color, duration));
            }

            await UniTask.WhenAll(tasks);
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