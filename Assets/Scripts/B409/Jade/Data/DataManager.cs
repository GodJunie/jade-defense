using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace B409.Jade.Data {
    public class DataManager : SingletonBehaviour<DataManager> {
        [AssetList(Path = "/Datas/Items", AutoPopulate = true)]
        [SerializeField]
        private List<ItemData> items;
        [AssetList(Path = "/Datas/Monsters", AutoPopulate = true)]
        [SerializeField]
        private List<MonsterData> monsters;
        [AssetList(Path = "/Datas/Stages", AutoPopulate = true)]
        [SerializeField]
        private List<StageData> stages;
             

        public List<ItemData> Items => items;
        public List<MonsterData> Monsters => monsters;
        public List<StageData> Stages => stages;
    }
}
