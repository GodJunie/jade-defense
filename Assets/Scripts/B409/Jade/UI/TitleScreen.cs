using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace B409.Jade.UI {
    using Game;

    public class TitleScreen : MonoBehaviour {
        [SerializeField]
        private float scrollSpeed = 3f;

        [SerializeField]
        private Image imageBlack;
        [SerializeField]
        private Image imageWhite;

        [SerializeField]
        private List<TMP_Text> texts;
        [SerializeField]
        private List<Transform> scales;

        [SerializeField]
        private Transform monsterHolder;

        private new Camera camera;
        private bool scroll;

        private void Awake() {
            camera = Camera.main;
            scroll = true;
        }

        private void Update() {
            if(scroll) {
                camera.transform.position += Vector3.right * Time.deltaTime * scrollSpeed;
            }
            monsterHolder.transform.position += Vector3.right * Time.deltaTime * scrollSpeed;
        }

        public async void NewGame() {
            await Fade();
            GameManager.Instance.NewGame();
        }

        public async void ContinueGame() {
            await Fade();
            GameManager.Instance.Continue();
        }

        private async UniTask Fade() {
            this.scroll = false;
            this.imageBlack.DOFade(0f, .5f);
            foreach(var text in texts) {
                text.DOFade(0f, .5f);
            }
            foreach(var t in scales) {
                t.DOScale(1.5f, .5f);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(2f));
            await this.imageWhite.DOFade(1f, 1f);
        }

        public void Quit() {
            Application.Quit();
        }
    }
}