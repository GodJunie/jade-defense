using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace B409.Jade.UI {

    public class BeforeAfterText : MonoBehaviour {
        [SerializeField]
        private TMP_Text textBefore;
        [SerializeField]
        private TMP_Text textAfter;

        public async UniTask Show(float before, float after, float duration) {
            var height = textBefore.GetComponent<RectTransform>().sizeDelta.y;

            textAfter.rectTransform.anchoredPosition = new Vector2(0f, after > before ? height : -height);
            textBefore.rectTransform.anchoredPosition = Vector2.zero;

            textBefore.text = before.ToString("0");
            textAfter.text = after.ToString("0");

            await UniTask.WhenAll(
                textAfter.rectTransform.DOLocalMoveY(0f, duration).ToUniTask(),
                textBefore.rectTransform.DOLocalMoveY(after > before ? -height : height, duration).ToUniTask()
            );
        }
    }
}