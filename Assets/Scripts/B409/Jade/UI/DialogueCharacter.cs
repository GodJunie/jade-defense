using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using System.Linq;

namespace B409.Jade.UI {

    public class DialogueCharacter : MonoBehaviour {
        [SerializeField]
        private SkeletonGraphic anim;
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

        public void Appear(bool isLeft) {
            this.gameObject.SetActive(true);

            if(this.isLeft != isLeft) {
                this.anim.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            } else {
                this.anim.transform.localRotation = Quaternion.identity;
            }

            this.anim.AnimationState.SetAnimation(0, appearAimation, false);
            this.anim.AnimationState.AddAnimation(0, idleAimation, true, 0f);
        }

        public async void SetFocus(bool focus, float duration = 0.5f) {
            Color originColor = this.anim.color;
            Color color = focus ? highlightedColor : disabledColor;

            for(float i = 0; i < duration; i += Time.fixedDeltaTime) {
                await UniTask.WaitForFixedUpdate();
                this.anim.color = originColor.Lerp(color, i / duration);
            }

            this.anim.color = color;
        }


#if UNITY_EDITOR
        public string[] animations {
            get {
                return this.anim.Skeleton.Data.Animations.Select(e => e.Name).ToArray();
            }
        }

        [Button]
        private void SetObject() {
            this.anim = this.transform.GetComponentInChildren<SkeletonGraphic>();
        }
#endif
    }
}