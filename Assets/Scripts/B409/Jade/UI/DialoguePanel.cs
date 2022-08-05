using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using TMPro;

namespace B409.Jade.UI {
    using Data;
    using Game;

    public class DialoguePanel : MonoBehaviour {
        [SerializeField]
        private bool isTest = false;
        [SerializeField]
        private DialogueData data;
        [SerializeField]
        private TMP_Text textName;
        [SerializeField]
        private TMP_Text textScript;
        [SerializeField]
        private Image imageBackground;
        [SerializeField]
        private Transform backgroundHolder;
        [SerializeField]
        private Transform leftCharacterHolder;
        [SerializeField]
        private Transform rightCharacterHolder;
        [SerializeField]
        private Image imageFade;

        private DialogueCharacter leftCharacter;
        private DialogueCharacter rightCharacter;

        private System.Threading.CancellationTokenSource click;

        private void Start() {
            // fade
            imageFade.gameObject.SetActive(true);
            imageFade.color = Color.white;
            imageFade.DOFade(0f, 1f);

            // test
            if(isTest) {

            } else {

            }

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
            }
            DialogueEnd();
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
            switch(sequence.SequenceSort) {
            case DialogueData.SequenceSort.Script:
                var left = sequence.LeftCharacter;
                var right = sequence.RightCharacter;
                var script = sequence.ScriptInfo;

                if(left.Mode == DialogueData.CharacterMode.Appear) {
                    for(int i = 0; i < leftCharacterHolder.childCount; i++) {
                        Destroy(leftCharacterHolder.GetChild(i).gameObject);
                    }

                    this.leftCharacter = Instantiate(left.Target, leftCharacterHolder.position, leftCharacterHolder.rotation, leftCharacterHolder);

                    leftCharacter.Appear(true);
                } else if(left.Mode == DialogueData.CharacterMode.Disappear) {
                    for(int i = 0; i < leftCharacterHolder.childCount; i++) {
                        Destroy(leftCharacterHolder.GetChild(i).gameObject);
                    }
                }

                if(right.Mode == DialogueData.CharacterMode.Appear) {
                    for(int i = 0; i < rightCharacterHolder.childCount; i++) {
                        Destroy(rightCharacterHolder.GetChild(i).gameObject);
                    }

                    this.rightCharacter = Instantiate(right.Target, rightCharacterHolder.position, rightCharacterHolder.rotation, rightCharacterHolder);
                    rightCharacter.Appear(false);
                } else if(right.Mode == DialogueData.CharacterMode.Disappear) {
                    for(int i = 0; i < rightCharacterHolder.childCount; i++) {
                        Destroy(rightCharacterHolder.GetChild(i).gameObject);
                    }
                }

                if(script.Name != "") {
                    this.textName.text = script.Name;
                }

                if(script.Script != "") {
                    leftCharacter?.SetFocus(script.ScriptFocus == DialogueData.ScriptFocus.Left);
                    rightCharacter?.SetFocus(script.ScriptFocus == DialogueData.ScriptFocus.Right);
                    
                    await PlayScript(script.Script, script.Speed, script.Skippable);
                    await GetWaitClick(1);
                }
                return;
            case DialogueData.SequenceSort.Background:
                switch(sequence.BackgroundMode) {
                case DialogueData.BackgroundMode.InGameBackground:
                    for(int i = 0; i < backgroundHolder.childCount; i++) {
                        Destroy(backgroundHolder.GetChild(i).gameObject);
                    }
                    Instantiate(sequence.BackgroundObject, Vector3.zero, Quaternion.identity, backgroundHolder);

                    this.imageBackground.gameObject.SetActive(false);
                    break;
                case DialogueData.BackgroundMode.Sprite:
                    this.imageBackground.sprite = sequence.BackgroundSprite;
                    this.imageBackground.gameObject.SetActive(true);
                    break;
                default:
                    break;
                }
                return;
            case DialogueData.SequenceSort.Effect:

                return;
            default:
                return;
            }
            
        }

        private async UniTask PlayScript(string script, float speed, bool skippable) {
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

        private async void DialogueEnd() {
            await imageFade.DOFade(1f, .5f);
            GameManager.Instance.StageSequenceEnd();
        }
    }
}