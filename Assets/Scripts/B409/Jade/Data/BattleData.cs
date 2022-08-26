using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    [CreateAssetMenu(fileName = "BattleData", menuName = "B409/Battle Data")]
    public class BattleData : ScriptableObject, IDataID {
        [BoxGroup("ID")]
        [HideLabel]
        [SerializeField]
        private int id;
        [BoxGroup("Settings")]
        [HorizontalGroup("Settings/group", .5f)]
        [BoxGroup("Settings/group/Background")]
        [HideLabel]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100f)]
        [SerializeField]
        private GameObject background;
        [HorizontalGroup("Settings/group", .5f)]
        [BoxGroup("Settings/group/Bgm")]
        [HideLabel]
        [PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 100f)]
        [SerializeField]
        private AudioClip bgm;


        [HorizontalGroup("group", .5f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<UnitData> enemies;
        [HorizontalGroup("group", .5f)]
        [ListDrawerSettings(Expanded = true, AddCopiesLastElement = true)]
        [SerializeField]
        private List<MaterialData> rewards;

        public int Id => id;
        public GameObject Background => background;
        public List<UnitData> Enemies => enemies;
        public List<MaterialData> Rewards => rewards;
        public AudioClip Bgm => bgm;
    }
}