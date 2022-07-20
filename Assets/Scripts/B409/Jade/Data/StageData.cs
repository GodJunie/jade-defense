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
        [SerializeField]
        private int id;
        // 현재 스테이지에서 등장하는 몬스터
        [SerializeField]
        private new string name;
        [SerializeField]
        private string description;
        [SerializeField]
        private Sprite background;

        [SerializeField]
        private List<MonsterData> monsters;
        [SerializeField]
        private List<BlockData> datas;


        public int Id => id;
        public string Name => name;
        public string Description => description;
        public Sprite Background => background;
        public List<MonsterData> Monsters => monsters;
        public List<BlockData> Datas => datas;
    }
}