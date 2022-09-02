using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    using UI;
    public class DialogueData : StageSequenceData {
#if UNITY_EDITOR
        [BoxGroup("Add")]
        [HorizontalGroup("Add/group")]
        [HideLabel]
        [MinValue(0)]
        [MaxValue("count")]
        [ShowInInspector]
        private int index;

        [HorizontalGroup("Add/group")]
        [HideLabel]
        [ShowInInspector]
        private int count {
            get {
                return this.datas.Count;
            }
        }

        [HorizontalGroup("Add/group")]
        [Button]
        private void Add() {
            var data = new DialogueSequenceData();
            this.datas.Insert(index, data);
        }

        private string DataErrorMsg {
            get {
                string msg = "";
                int left = -1;
                int right = -1;
                bool obj = false;

                for(int i = 0; i < datas.Count; i++) {
                    var data = datas[i];
                    switch(data.SequenceSort) {
                    case SequenceSort.Script:
                        switch(data.LeftCharacter.Mode) {
                        case CharacterMode.Appear:
                            if(left > 0) {
                                return string.Format("Index: {0}, (Left) There is loaded target already. (Loaded target on index : {1})", i, left);
                            }
                            if(data.LeftCharacter == null) {
                                return string.Format("Index: {0}, (Left) Set target to appear", i);
                            }
                            left = i;
                            break;
                        case CharacterMode.Disappear:
                            if(left > 0) {
                                left = -1;
                            } else {
                                return string.Format("Index: {0}, (Left) There is no target to disappear", i);
                            }
                            break;
                        }
                        switch(data.RightCharacter.Mode) {
                        case CharacterMode.Appear:
                            if(right > 0) {
                                return string.Format("Index: {0}, (Right) There is loaded target already. (Loaded target on index : {1})", i, right);
                            }
                            if(data.RightCharacter == null) {
                                return string.Format("Index: {0}, (Right) Set target to appear", i);
                            }
                            right = i;
                            break;
                        case CharacterMode.Disappear:
                            if(right > 0) {
                                right = -1;
                            } else {
                                return string.Format("Index: {0}, (Right) There is no target to disappear", i);
                            }
                            break;
                        }

                        if(data.ScriptInfo.ScriptFocus == ScriptFocus.Left && left < 0) {
                            return string.Format("Index: {0}, (Left) No target to focus", i);
                        }
                        if(data.ScriptInfo.ScriptFocus == ScriptFocus.Right && right < 0) {
                            return string.Format("Index: {0}, (Right) No target to focus", i);
                        }
                        break;
                    case SequenceSort.Object:
                        if(data.EventObject != null) {
                            obj = true;
                        }
                        if(!string.IsNullOrEmpty(data.EventId) && !obj) {
                            return string.Format("Index: {0}, No object to play event", i);
                        }
                        break;
                    case SequenceSort.Sound:
                        if(data.Sfx == null && data.Bgm == null) {
                            return string.Format("Index: {0}, No clip to play", i);
                        }
                        break;
                    }
                }

                return msg;
            }
        }

        private bool DataIsError {
            get {
                return !string.IsNullOrEmpty(DataErrorMsg);
            }
        }
#endif

        [ListDrawerSettings(AddCopiesLastElement = false, DraggableItems = true, Expanded = true, NumberOfItemsPerPage = 20, ShowIndexLabels = true, ListElementLabelName = "Summary")]
        [InfoBox("$DataErrorMsg", InfoMessageType.Error, "DataIsError")]
        [SerializeField]
        private List<DialogueSequenceData> datas;

        public List<DialogueSequenceData> Datas => datas;

        [System.Serializable]
        public class DialogueSequenceData {
            [BoxGroup("box", false)]
            [HorizontalGroup("box/group", 100f)]
            [BoxGroup("box/group/Sort")]
            [HideLabel]
            [SerializeField]
            private SequenceSort sequenceSort = SequenceSort.Script;

            [HorizontalGroup("box/group")]
            [ShowIfGroup("box/group/ShowScript")]
            [BoxGroup("box/group/ShowScript/Script")]
            [HorizontalGroup("box/group/ShowScript/Script/group", 150f, MaxWidth = 150f)]
            [HideLabel]
            [GUIColor("LeftColor")]
            [SerializeField]
            private CharacterInfo leftCharacter;

            [HorizontalGroup("box/group/ShowScript/Script/group")]
            [HideLabel]
            [SerializeField]
            [GUIColor("ScriptColor")]
            private ScriptInfo scriptInfo;

            [HorizontalGroup("box/group/ShowScript/Script/group", 150f, MaxWidth = 150f)]
            [HideLabel]
            [GUIColor("RightColor")]
            [SerializeField]
            private CharacterInfo rightCharacter;

            [HorizontalGroup("box/group")]
            [ShowIfGroup("box/group/ShowObject")]
            [BoxGroup("box/group/ShowObject/Objects")]
            [HorizontalGroup("box/group/ShowObject/Objects/group", 100f)]
            [BoxGroup("box/group/ShowObject/Objects/group/Target")]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 80f)]
            [HideLabel]
            [SerializeField]
            private DialogueEvent eventObject;

            [HorizontalGroup("box/group/ShowObject/Objects/group")]
            [BoxGroup("box/group/ShowObject/Objects/group/Id")]
            [HideLabel]
            [SerializeField]
            private string eventId;

            [HorizontalGroup("box/group")]
            [ShowIfGroup("box/group/ShowReward")]
            [BoxGroup("box/group/ShowReward/Rewards")]
            [SerializeField]
            private List<RewardInfo> rewards;


            [HorizontalGroup("box/group")]
            [ShowIfGroup("box/group/ShowSound")]
            [BoxGroup("box/group/ShowSound/Sounds")]
            [HorizontalGroup("box/group/ShowSound/Sounds/group")]
            [BoxGroup("box/group/ShowSound/Sounds/group/Bgm")]
            [HideLabel]
            [SerializeField]
            private AudioClip bgm;
            [HorizontalGroup("box/group/ShowSound/Sounds/group")]
            [BoxGroup("box/group/ShowSound/Sounds/group/Sfx")]
            [HideLabel]
            [SerializeField]
            private AudioClip sfx;
            [HorizontalGroup("box/group/ShowSound/Sounds/group")]
            [BoxGroup("box/group/ShowSound/Sounds/group/BgmOff")]
            [HideLabel]
            [SerializeField]
            private bool bgmOff = false;


            [HorizontalGroup("box/group")]
            [ShowIfGroup("box/group/ShowEtc")]
            [BoxGroup("box/group/ShowEtc/Etc")]
            [HorizontalGroup("box/group/ShowEtc/Etc/group")]
            [BoxGroup("box/group/ShowEtc/Etc/group/Lock Monster")]
            [HideLabel]
            [SerializeField]
            private bool lockMonster;

            [HorizontalGroup("box/group/ShowEtc/Etc/group")]
            [BoxGroup("box/group/ShowEtc/Etc/group/Unlock Monster")]
            [HideLabel]
            [SerializeField]
            private bool unlockMonster;

            public CharacterInfo LeftCharacter => leftCharacter;
            public ScriptInfo ScriptInfo => scriptInfo;
            public CharacterInfo RightCharacter => rightCharacter;
            public SequenceSort SequenceSort => sequenceSort;
            public DialogueEvent EventObject => eventObject;
            public string EventId => eventId;
            public List<RewardInfo> Rewards => rewards;
            public AudioClip Bgm => bgm;
            public AudioClip Sfx => sfx;
            public bool BgmOff => bgmOff;
            public bool LockMonster => lockMonster;
            public bool UnlockMonster => unlockMonster;

            public DialogueSequenceData() {
                this.leftCharacter = new CharacterInfo();
                this.rightCharacter = new CharacterInfo();
                this.scriptInfo = new ScriptInfo();
                this.rewards = new List<RewardInfo>();
            }

#if UNITY_EDITOR
            private Color ScriptColor {
                get {
                    switch(scriptInfo.ScriptFocus) {
                    case ScriptFocus.Left:
                        return LeftColor;
                    case ScriptFocus.Right:
                        return RightColor;
                    case ScriptFocus.None:
                        return Color.white;
                    default:
                        return Color.white;
                    }
                }
            }

            private Color LeftColor {
                get {
                    return new Color32(255, 156, 156, 255);
                }
            }

            private Color RightColor {
                get {
                    return new Color32(176, 207, 255, 255);
                }
            }

            #region Flags
            private bool ShowScript {
                get {
                    return sequenceSort == SequenceSort.Script;
                }
            }

            private bool ShowObject {
                get {
                    return sequenceSort == SequenceSort.Object;
                }
            }

            private bool ShowReward {
                get {
                    return sequenceSort == SequenceSort.Reward;
                }
            }

            private bool ShowSound {
                get {
                    return sequenceSort == SequenceSort.Sound;
                }
            }

            private bool ShowEtc {
                get {
                    return sequenceSort == SequenceSort.Etc;
                }
            }
            #endregion

            private string Summary {
                get {
                    switch(this.sequenceSort) {
                    case SequenceSort.Script:
                        string left = "-";
                        if(leftCharacter.Mode == CharacterMode.Appear) {
                            left = string.Format("Appear {0}", leftCharacter.Target ? leftCharacter.Target.name : "-");
                        }
                        if(leftCharacter.Mode == CharacterMode.Disappear) {
                            left = "Disappear";
                        }
                        if(leftCharacter.Mode == CharacterMode.Animation) {
                            left = string.Format("Animation {0}", leftCharacter.Animation);
                        }

                        string right = "-";
                        if(rightCharacter.Mode == CharacterMode.Appear) {
                            right = string.Format("Appear {0}", rightCharacter.Target ? rightCharacter.Target.name : "-");
                        }
                        if(rightCharacter.Mode == CharacterMode.Disappear) {
                            right = "Disappear";
                        }
                        if(rightCharacter.Mode == CharacterMode.Animation) {
                            right = string.Format("Animation {0}", rightCharacter.Animation);
                        }

                        return string.Format("{0} / {1} : {2} / {3}", left, scriptInfo.Name, scriptInfo.Script, right);
                    case SequenceSort.Object:
                        if(this.eventObject != null) {
                            return string.Format("Load Event Object : {0}", eventObject.name);
                        }
                        if(!string.IsNullOrEmpty(this.eventId)) {
                            return string.Format("Play Event Id: {0}", eventId);
                        }
                        return "";
                    case SequenceSort.Reward:
                        string str = "Rewards";
                        foreach(var reward in this.rewards) {
                            if(reward.Data != null && reward.Data is IDataID)
                                str += string.Format("/id: {0}, count: {1}", (reward.Data as IDataID).Id, reward.Count);
                        }
                        return str;
                    case SequenceSort.Sound:
                        string soundStr = "Sounds";
                        if(bgm != null)
                            soundStr += string.Format(" / Bgm: {0}", bgm.name);
                        if(sfx != null)
                            soundStr += string.Format(" / Sfx: {0}", sfx.name);
                        return soundStr;
                    case SequenceSort.Etc:
                        if(lockMonster)
                            return "Lock Monsters";
                        if(unlockMonster)
                            return "Unlock Monsters";
                        return "";
                    default:
                        return "";
                    }
                }
            }
#endif
        }

        #region Define
        [System.Serializable]
        public class CharacterInfo {
            [BoxGroup("Mode")]
            [HideLabel]
            [SerializeField]
            private CharacterMode mode;

            [ShowIf("@this.mode==CharacterMode.Appear")]
            [BoxGroup("Target")]
            [HideLabel]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100f)]
            [AssetList(Path = "/Prefabs/Dialogue")]
            [SerializeField]
            private DialogueCharacter target;

            [ShowIf("@this.mode==CharacterMode.Animation")]
            [BoxGroup("Animation")]
            [HideLabel]
            [SerializeField]
            private string animation;

            public CharacterMode Mode => mode;
            public DialogueCharacter Target => target;
            public string Animation => animation;
        }

        [System.Serializable]
        public class ScriptInfo {
#if UNITY_EDITOR
            [VerticalGroup("group")]
            [ButtonGroup("group/buttons")]
            [Button("Left")]
            private void SetLeft() {
                this.scriptFocus = ScriptFocus.Left; 
            }

            [ButtonGroup("group/buttons")]
            [Button("None")]
            private void SetNone() {
                this.scriptFocus = ScriptFocus.None;
            }

            [ButtonGroup("group/buttons")]
            [Button("Right")]
            private void SetRight() {
                this.scriptFocus = ScriptFocus.Right;
            }
#endif

            [HideInInspector]
            [SerializeField]
            private ScriptFocus scriptFocus;

            [VerticalGroup("group")]
            [HorizontalGroup("group/group", .2f, MaxWidth = 100f)]
            [BoxGroup("group/group/Name")]
            [HideLabel]
            [SerializeField]
            private string name;
            [HorizontalGroup("group/group")]
            [BoxGroup("group/group/Script")]
            [HideLabel]
            [TextArea(0, 3)]
            [SerializeField]
            private string script;

            [VerticalGroup("group")]
            [BoxGroup("group/Settings")]
            [HorizontalGroup("group/Settings/group", .5f, MinWidth = 120f)]
            [LabelWidth(80f)]
            [SerializeField]
            private float speed = 20;
            [HorizontalGroup("group/Settings/group", .5f, MinWidth = 120f)]
            [LabelWidth(80f)]
            [SerializeField]
            private bool skippable = true;


            public string Name => name;
            public string Script => script;
            public float Speed => speed;
            public bool Skippable => skippable;
            public ScriptFocus ScriptFocus => scriptFocus;
        }

        [System.Serializable]
        public class RewardInfo {
            [HorizontalGroup("group", .5f)]
            [BoxGroup("group/Data")]
            [HideLabel]
            [SerializeField]
            private ScriptableObject data;
            [HorizontalGroup("group", .5f)]
            [BoxGroup("group/Count")]
            [HideLabel]
            [SerializeField]
            private int count;

            public ScriptableObject Data => data;
            public int Count => count;
        }

        public enum ScriptFocus : int { Left, Right, None };
        public enum SequenceSort : int { Script = 0, Object = 3, Reward, Sound, Etc };
        public enum CharacterMode : int { None, Appear, Disappear, Animation };
        #endregion

    }
}