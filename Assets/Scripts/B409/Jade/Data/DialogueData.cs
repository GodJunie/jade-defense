using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    public class DialogueData : StageSequenceData {
        [ListDrawerSettings(AddCopiesLastElement =false, DraggableItems = false, Expanded = true, NumberOfItemsPerPage = 100)]
        [SerializeField]
        private List<DialogueSequenceData> datas;

        public List<DialogueSequenceData> Datas => datas;

        [System.Serializable]
        public class DialogueSequenceData {
            [FoldoutGroup("@Summary")]
            [HorizontalGroup("@Summary/group", .1f, MaxWidth = 200f)]
            [HideLabel]
            [SerializeField]
            private CharacterInfo leftCharacter;

            [HorizontalGroup("@Summary/group", .5f)]
            [HideLabel]
            [SerializeField]
            private ScriptInfo script;

            [HorizontalGroup("@Summary/group", .1f, MaxWidth = 200f)]
            [HideLabel]
            [SerializeField]
            private CharacterInfo rightCharacter;


            public CharacterInfo LeftCharacter => leftCharacter;
            public ScriptInfo Script => script;
            public CharacterInfo RightCharacter => rightCharacter;

            public enum CharacterMode : int { None, Appear, Disappear };

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
                [SerializeField]
                private GameObject target;

                public CharacterMode Mode => mode;
                public GameObject Target => target;
            }
       
            [System.Serializable]
            public class ScriptInfo {
                [HorizontalGroup("group", .2f, MaxWidth = 100f)]
                [BoxGroup("group/Name")]
                [HideLabel]
                [SerializeField]
                private string name;
                [HorizontalGroup("group")]
                [BoxGroup("group/Script")]
                [HideLabel]
                [TextArea(0, 3)]
                [SerializeField]
                private string script;
                [BoxGroup("Settings")]
                [HorizontalGroup("Settings/group", .5f, MinWidth = 120f)]
                [LabelWidth(80f)]
                [SerializeField]
                private float speed;
                [HorizontalGroup("Settings/group", .5f, MinWidth = 120f)]
                [LabelWidth(80f)]
                [SerializeField]
                private bool skippable;

                public string Name => name;
                public string Script => script;
                public float Speed => speed;
                public bool Skippable => skippable;
            }

#if UNITY_EDITOR
            private string Summary {
                get {
                    string left = leftCharacter.Mode == CharacterMode.None ? "-" : leftCharacter.Mode == CharacterMode.Appear ? string.Format("Appear {0}", leftCharacter.Target ? leftCharacter.Target.name : "-") : "Disappear";

                    string right = rightCharacter.Mode == CharacterMode.None ? "-" : rightCharacter.Mode == CharacterMode.Appear ? string.Format("Appear {0}", rightCharacter.Target ? rightCharacter.Target.name : "-") : "Disappear";

                    return string.Format("{0} / {1} : {2} / {3}", left, script.Name, script.Script, right);
                }
            }
#endif
        }
    }
}