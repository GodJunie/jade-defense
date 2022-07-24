// System
using System.Collections;
using System.Collections.Generic;
// UnityEngine
using UnityEngine;
// Editor
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "StageData", menuName = "B409/Stage Data")]
    public class StageData : ScriptableObject, IDataID {
        [BoxGroup("Id")]
        [HideLabel]
        [SerializeField]
        private int id;
        // 현재 스테이지에서 등장하는 몬스터
        [HorizontalGroup("group", 200f)]
        [BoxGroup("group/Background")]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 200f)]
        [HideLabel]
        [SerializeField]
        private Sprite background;
        [HorizontalGroup("group", 200f)]
        [BoxGroup("group/InGameBackground")]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 200f)]
        [HideLabel]
        [SerializeField]
        private GameObject inGameBackground;

        [HorizontalGroup("group")]
        [VerticalGroup("group/group")]
        [BoxGroup("group/group/Name")]
        [HideLabel]
        [SerializeField]
        private new string name;
        [HorizontalGroup("group")]
        [BoxGroup("group/group/Description")]
        [TextArea(0, 3)]
        [HideLabel]
        [SerializeField]
        private string description;
       
        [BoxGroup("Sequence")]
        [SerializeField]
        private List<StageSequenceData> datas;


        public int Id => id;
        public string Name => name;
        public string Description => description;
        public Sprite Background => background;
        public List<StageSequenceData> Datas => datas;
    }
}