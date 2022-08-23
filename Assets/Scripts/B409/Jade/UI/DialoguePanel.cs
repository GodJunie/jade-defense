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
        private Transform leftCharacterHolder;
        [SerializeField]
        private Transform rightCharacterHolder;
        [SerializeField]
        private Image imageFade;
        [SerializeField]
        private Transform chatObject;
        [SerializeField]
        private float defaultWaitDuration = 1f;
        [SerializeField]
        private Button buttonSkip;

        private DialogueEvent eventObject;
        private bool chatOn = true;
        private DialogueCharacter leftCharacter;
        private DialogueCharacter rightCharacter;

        private System.Threading.CancellationTokenSource click;
        private List<Graphic> chatGraphics = new List<Graphic>();
        private Dictionary<int, float> chatAlphaOrigin = new Dictionary<int, float>();


        private void Awake() {
            chatObject.gameObject.SetActive(true);
            var graphics = chatObject.GetComponentsInChildren<Graphic>();
            foreach(var graphic in graphics) {
                var color = graphic.color;
                this.chatGraphics.Add(graphic);
                this.chatAlphaOrigin.Add(graphic.GetInstanceID(), color.a);
                color.a = 0f;
                graphic.color = color;
            }
            chatOn = false;
        }

        private void Start() {
            if(isTest) {

            } else {
                this.data = GameManager.Instance.CurrentStageSequence as DialogueData;
            }

            // fade
            FadeIn();

            Open();
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0)) {
                if(click != null)
                    click.Cancel();
            }
        }

        private async void FadeIn() {
            imageFade.gameObject.SetActive(true);
            imageFade.color = Color.white;
            await imageFade.DOFade(0f, 1f);
            imageFade.gameObject.SetActive(false);

            bool canSkip = GameManager.Instance.StoryCollection.Contains(data.Id);

            buttonSkip.gameObject.SetActive(canSkip);
        }

        private async UniTask FadeInChat(float duration = 0.5f) {
            this.chatOn = true;
            this.textScript.text = "";
            this.textName.text = "";

            var tasks = new List<UniTask>();

            foreach(var graphic in chatGraphics) {
                float alpha = chatAlphaOrigin[graphic.GetInstanceID()];
                tasks.Add(graphic.DOFade(alpha, duration).ToUniTask());
            }

            if(leftCharacter != null) {
                tasks.Add(leftCharacter.FadeIn(duration));
            }
            if(rightCharacter != null) {
                tasks.Add(rightCharacter.FadeIn(duration));
            }

            await UniTask.WhenAll(tasks);
        }

        private async UniTask FadeOutChat(float duration = 0.5f) {
            this.chatOn = false;

            var tasks = new List<UniTask>();

            foreach(var graphic in chatGraphics) {
                tasks.Add(graphic.DOFade(0f, duration).ToUniTask());
            }

            if(leftCharacter != null) {
                tasks.Add(leftCharacter.FadeOut(duration));
            }
            if(rightCharacter != null) {
                tasks.Add(rightCharacter.FadeOut(duration));
            }

            await UniTask.WhenAll(tasks);
        }

        public async void Open() {
            foreach(var sequence in data.Datas) {
                await PlaySequence(sequence);
            }
            DialogueEnd();
        }

        private async UniTask WaitClick(float delay) {
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
                if(!chatOn) {
                    await FadeInChat();
                }

                var left = sequence.LeftCharacter;
                var right = sequence.RightCharacter;
                var script = sequence.ScriptInfo;

                var tasks = new List<UniTask>();

                if(left.Mode == DialogueData.CharacterMode.Appear) {
                    if(leftCharacter != null) {
                        throw new Exception(string.Format("Left character is already exists"));
                    }

                    this.leftCharacter = Instantiate(left.Target, leftCharacterHolder.position, leftCharacterHolder.rotation, leftCharacterHolder);

                    leftCharacter.Appear(true);
                    tasks.Add(leftCharacter.SetFocus(true));
                } else if(left.Mode == DialogueData.CharacterMode.Disappear) {
                    if(leftCharacter == null) {
                        throw new Exception("No left character exists to disappear");
                    }

                    tasks.Add(leftCharacter.SetFocus(false).ContinueWith(() => {
                        Destroy(leftCharacter.gameObject);
                        leftCharacter = null;
                    }));
                } else if(left.Mode == DialogueData.CharacterMode.Animation) {
                    if(leftCharacter == null) {
                        throw new Exception("No left character exists to play animation");
                    }

                    leftCharacter.PlayAnimation(left.Animation);
                }

                if(right.Mode == DialogueData.CharacterMode.Appear) {
                    if(rightCharacter != null) {
                        throw new Exception(string.Format("Right character is already exists"));
                    }

                    this.rightCharacter = Instantiate(right.Target, rightCharacterHolder.position, rightCharacterHolder.rotation, rightCharacterHolder);

                    rightCharacter.Appear(false);
                    tasks.Add(rightCharacter.SetFocus(true));
                } else if(right.Mode == DialogueData.CharacterMode.Disappear) {
                    if(rightCharacter == null) {
                        throw new Exception("No right character exists to disappear");
                    }

                    tasks.Add(rightCharacter.SetFocus(false).ContinueWith(() => {
                        Destroy(rightCharacter.gameObject);
                        rightCharacter = null;
                    }));
                } else if(right.Mode == DialogueData.CharacterMode.Animation) {
                    if(rightCharacter == null) {
                        throw new Exception("No right character exists to play animation");
                    }

                    rightCharacter.PlayAnimation(right.Animation);
                }

                if(tasks.Count > 0)
                    await UniTask.WhenAll(tasks);

                if(script.Name != "") {
                    this.textName.text = script.Name;
                }

                if(script.Script != "") {
                    if(leftCharacter != null) {
                        leftCharacter.SetFocus(script.ScriptFocus == DialogueData.ScriptFocus.Left);
                    }
                    if(rightCharacter != null) {
                        rightCharacter.SetFocus(script.ScriptFocus == DialogueData.ScriptFocus.Right);
                    }

                    await PlayScript(script.Script, script.Speed, script.Skippable);
                    await WaitClick(defaultWaitDuration);
                }
                return;
            case DialogueData.SequenceSort.Object:
                if(sequence.EventObject != null) {
                    this.eventObject = Instantiate(sequence.EventObject, Vector3.zero, Quaternion.identity);
                    await WaitClick(defaultWaitDuration);
                }
                if(!string.IsNullOrEmpty(sequence.EventId)) {
                    if(chatOn) {
                        await FadeOutChat();
                    }
                    await this.eventObject.WaitEvent(sequence.EventId);
                    await WaitClick(defaultWaitDuration);
                }
                return;
            case DialogueData.SequenceSort.Effect:

                return;
            case DialogueData.SequenceSort.Reward:
                foreach(var reward in sequence.Rewards) {
                    var data = reward.Data;
                    if(data is ItemData) {
                        GameManager.Instance.Progress.AddItem((data as ItemData).Id, reward.Count);
                    } else if(data is MonsterData) {
                        GameManager.Instance.Progress.AddMonster((data as MonsterData).Id, reward.Count);
                    } else {
                        throw new Exception(string.Format("Data is not item neither monster, name: {0}", data.name));
                    }
                }
                return;
            default:
                return;
            }
        }

        private async UniTask PlayScript(string script, float speed, bool skippable) {
            this.textScript.text = "";
            var task = this.textScript.DOText(script, speed).SetSpeedBased(true).ToUniTask();

            if(skippable) {
                var waitClick = WaitClick(defaultWaitDuration);
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

        public async void DialogueEnd() {
            imageFade.gameObject.SetActive(true);
            await imageFade.DOFade(1f, 1f);
            GameManager.Instance.StageSequenceEnd();
        }
    }
}