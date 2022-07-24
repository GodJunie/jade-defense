using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace B409.Jade.UI {
    using Data;

    public class DialoguePanel : MonoBehaviour {
        [SerializeField]
        private DialogueData data;
        [SerializeField]
        private Text textName;
        [SerializeField]
        private Text textScript;

        private System.Threading.CancellationTokenSource click;

        private void Start() {
            Open();
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0)) {
                if(click != null)
                    click.Cancel();
            }
        }

        public async void Open() {
            foreach(var sequence in data.Datas) {
                await PlaySequence(sequence);
                await GetWaitClick(1);
            }
        }

        private async UniTask GetWaitClick(float delay) {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if(click != null) {
                click.Dispose();
            }
            click = new System.Threading.CancellationTokenSource();
            await UniTask.WaitUntilCanceled(click.Token);
        }

        private async UniTask PlaySequence(DialogueData.DialogueSequenceData sequence) {
            var left = sequence.LeftCharacter;
            var right = sequence.RightCharacter;
            var script = sequence.Script;

            if(script.Name != "") {
                this.textName.text = script.Name;
            }

            if(script.Script != "") {
                await PlayScript(script.Script, script.Speed, script.Skippable);
            }
        }

        private async UniTask PlayScript(string script, float speed, bool skippable) {
            if(speed == 0)
                speed = 10;

            this.textScript.text = "";
            var task = this.textScript.DOText(script, speed).SetSpeedBased(true).ToUniTask();

            if(skippable) {
                var waitClick = GetWaitClick(1);
                var result = await UniTask.WhenAny(waitClick, task);  
                if(result == 0) {
                    Debug.Log("Clicked");
                    this.textScript.DOKill();
                    this.textScript.text = script;
                    return;
                }
            } else {
                await task;
            }
        }
    }
}