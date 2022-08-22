using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace B409.Jade.Data {
    public class DataManager : SingletonBehaviour<DataManager> {
        [AssetList(Path = "/Datas/Items", AutoPopulate = true)]
        [SerializeField]
        private List<ItemData> items;
        [AssetList(Path = "/Datas/Monsters", AutoPopulate = true)]
        [SerializeField]
        private List<MonsterData> monsters;

        [SerializeField]
        private List<int> stages;
        [SerializeField]
        private string stageDataPath;
        [SerializeField]
        private string stageSequenceDataPath;

        public StageData LoadStageData(int id) {
            var data = Resources.Load<ScriptableObject>(Path.Combine(stageDataPath, id.ToString()));

            return data as StageData;
        }

        public async UniTask<StageData> LoadStageDataAsync(int id) {
            var data = await Resources.LoadAsync<ScriptableObject>(Path.Combine(stageDataPath, id.ToString()));

            return data as StageData;
        }

        public StageSequenceData LoadStageSequenceData(string path) {
            var data = Resources.Load<ScriptableObject>(Path.Combine(stageSequenceDataPath, path));

            return data as StageSequenceData;
        }

        public async UniTask<StageSequenceData> LoadStageSequenceDataAsync(string path) {
            var data = await Resources.LoadAsync<ScriptableObject>(Path.Combine(stageSequenceDataPath, path));

            return data as StageSequenceData;
        }

        public List<ItemData> Items => items;
        public List<MonsterData> Monsters => monsters;
        public List<int> Stages => stages;
    }
}
