using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    using UI;
    public class DialogueData : StageSequenceData {
        [ListDrawerSettings(AddCopiesLastElement = false, DraggableItems = true, Expanded = true, NumberOfItemsPerPage = 100)]
        [SerializeField]
        private List<DialogueSequenceData> datas;

        public List<DialogueSequenceData> Datas => datas;

        [System.Serializable]
        public class DialogueSequenceData {
            [FoldoutGroup("@Summary")]
            [HorizontalGroup("@Summary/group")]
            [ShowIfGroup("@Summary/group/ShowScript")]
            [HorizontalGroup("@Summary/group/ShowScript/group", 150f, MaxWidth = 150f)]
            [HideLabel]
            [GUIColor("LeftColor")]
            [SerializeField]
            private CharacterInfo leftCharacter;

            [HorizontalGroup("@Summary/group/ShowScript/group")]
            [HideLabel]
            [SerializeField]
            [GUIColor("ScriptColor")]
            private ScriptInfo scriptInfo;

            [HorizontalGroup("@Summary/group/ShowScript/group", 150f, MaxWidth = 150f)]
            [HideLabel]
            [GUIColor("RightColor")]
            [SerializeField]
            private CharacterInfo rightCharacter;


            [HorizontalGroup("@Summary/group")]
            [ShowIfGroup("@Summary/group/ShowBackground")]
            [HorizontalGroup("@Summary/group/ShowBackground/group", .1f, MaxWidth = 200f)]
            [BoxGroup("@Summary/group/ShowBackground/group/Mode")]
            [HideLabel]
            [SerializeField]
            private BackgroundMode backgroundMode;


            [HorizontalGroup("@Summary/group/ShowBackground/group", .1f, MaxWidth = 200f)]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100f)]
            [BoxGroup("@Summary/group/ShowBackground/group/Target")]
            [ShowIf("InGameBackground")]
            [HideLabel]
            [SerializeField]
            private GameObject backgroundObject;

            [HorizontalGroup("@Summary/group/ShowBackground/group", .1f, MaxWidth = 200f)]
            [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100f)]
            [BoxGroup("@Summary/group/ShowBackground/group/Target")]
            [HideIf("InGameBackground")]
            [HideLabel]
            [SerializeField]
            private Sprite backgroundSprite;


            [HorizontalGroup("@Summary/group", MaxWidth = 100f)]
            [BoxGroup("@Summary/group/Sort")]
            [HideLabel]
            [SerializeField]
            private SequenceSort sequenceSort = SequenceSort.Script;


            public CharacterInfo LeftCharacter => leftCharacter;
            public ScriptInfo ScriptInfo => scriptInfo;
            public CharacterInfo RightCharacter => rightCharacter;
            public SequenceSort SequenceSort => sequenceSort;
            public BackgroundMode BackgroundMode => backgroundMode;
            public GameObject BackgroundObject => backgroundObject;
            public Sprite BackgroundSprite => backgroundSprite;

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

            private bool ShowScript {
                get {
                    return sequenceSort == SequenceSort.Script;
                }
            }

            private bool ShowBackground {
                get {
                    return sequenceSort == SequenceSort.Background;
                }
            }

            private bool InGameBackground {
                get {
                    return this.backgroundMode == BackgroundMode.InGameBackground;
                }
            }

            private string Summary {
                get {
                    switch(this.sequenceSort) {
                    case SequenceSort.Script:
                        string left = leftCharacter.Mode == CharacterMode.None ? "-" : leftCharacter.Mode == CharacterMode.Appear ? string.Format("Appear {0}", leftCharacter.Target ? leftCharacter.Target.name : "-") : "Disappear";

                        string right = rightCharacter.Mode == CharacterMode.None ? "-" : rightCharacter.Mode == CharacterMode.Appear ? string.Format("Appear {0}", rightCharacter.Target ? rightCharacter.Target.name : "-") : "Disappear";

                        return string.Format("{0} / {1} : {2} / {3}", left, scriptInfo.Name, scriptInfo.Script, right);

                    case SequenceSort.Background:
                        if(this.backgroundMode == BackgroundMode.InGameBackground) {
                            return string.Format("InGameBackground: {0}", this.backgroundObject ? this.backgroundObject.name : "-");
                        }
                        if(this.backgroundMode == BackgroundMode.Sprite) {
                            return string.Format("TextureBackground: {0}", this.backgroundSprite ? this.backgroundSprite.name : "-");
                        }
                        return "";
                    case SequenceSort.Effect:
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

            public CharacterMode Mode => mode;
            public DialogueCharacter Target => target;
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

        public enum ScriptFocus : int { Left, Right, None };
        public enum BackgroundMode : int { InGameBackground, Sprite };
        public enum SequenceSort : int { Script, Background, Effect };
        public enum CharacterMode : int { None, Appear, Disappear };
        #endregion
    }
}