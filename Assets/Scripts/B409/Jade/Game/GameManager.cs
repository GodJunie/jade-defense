using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

namespace B409.Jade.Game {
    using Data;

    public class GameManager : SingletonBehaviour<GameManager> {
        #region Consts 
        private const string GameProgressKey = "GameProgress";
        private const string StoryCollectionKey = "StoryCollection";
        #endregion

        #region SerializedField
        [SerializeField]
        private string titleScene;
        [SerializeField]
        private string mainScene;
        [SerializeField]
        private string battleScene;
        [SerializeField]
        private string dialogueScene;
        [SerializeField]
        private string endingScene;
        #endregion

        #region Members 
        public GameProgress Progress { get; private set; }
        public List<int> StoryCollection { get; private set; }
        #endregion

        #region Mono
        protected override void Awake() {
            base.Awake();

            string json = PlayerPrefs.GetString(StoryCollectionKey, "[]");
            this.StoryCollection = JsonConvert.DeserializeObject<List<int>>(json);
        }
        #endregion

        #region Load
        public void NewGame() {
            Progress = new GameProgress();
            StageSequenceStart();
            Save();
            LoadScene();
        }

        public void Continue() {
            LoadScene();
        }

        public void Retry() {
            string json = PlayerPrefs.GetString(GameProgressKey, "");
         
            this.Progress = GameProgress.FromJson(json);

            LoadData();
            Continue();
        }

        public void Save() {
            string json = this.Progress.ToJson();
            PlayerPrefs.SetString(GameProgressKey, json);
        }
        #endregion

        #region Game
        public StageData CurrentStage { get; private set; }

        public StageSequenceData CurrentStageSequence { get; private set; }

        public void TurnStart() {
            Progress.TurnStart();
        }

        public void TurnEnd() {
            if(!(CurrentStageSequence is DailyRoutineData))
                return;

            Progress.TurnEnd();

            LoadScene();
        }

        public void StageSequenceEnd() {
            if(CurrentStageSequence is DialogueData) {
                StoryCollection.Add(CurrentStageSequence.Id);
                var json = JsonConvert.SerializeObject(StoryCollection);
                Debug.Log(json);
                PlayerPrefs.SetString(StoryCollectionKey, json);
            }

            Progress.NextStageSequence();

            if(Progress.StageSequence == CurrentStage.Datas.Count) {
                // Stage Clear!
                Progress.NextStage();
            }

            if(Progress.Stage == DataManager.Instance.Stages.Count) {
                LoadTitleScene();
                return;
            }

            StageSequenceStart();
            Save();
            LoadScene();
        }

        private void StageSequenceStart() {
            if(Progress.Stage == DataManager.Instance.Stages.Count) {
                return;
            }

            LoadData();

            if(CurrentStageSequence is DailyRoutineData) {
                Progress.DailyRoutineStart(CurrentStageSequence as DailyRoutineData);
            }
        }
        #endregion

        #region Farming
        public void Farming(FarmingLevelData data) {
            int count = GameConsts.GetFarmingCount(Progress.Parameters[Parameter.Luck]);

            var items = new Dictionary<int, int>();

            for(int i = 0; i < count; i++) {
                Debug.Log("get");

                var sum = data.Datas.Sum(e => e.Rate);
                var rand = UnityEngine.Random.Range(0f, sum);

                foreach(var rateData in data.Datas) {
                    if(rand < rateData.Rate) {
                        var item = rateData.Item;

                        if(item != null) {
                            if(items.ContainsKey(item.Id))
                                items[item.Id] += 1;
                            else
                                items.Add(item.Id, 1);
                        }
                       
                        break;
                    }

                    rand -= rateData.Rate;
                }
            }

            Progress.AddItems(items);
            CheckAction(data);
        }

        public void Making(ActionLevelData levelData, RecipeData data) {
            if(!Progress.CheckItemsEnough(data.Materials))
                return;

            bool bonus = UnityEngine.Random.Range(0f, 100f) < GameConsts.GetCraftingBonusRate(Progress.Parameters[Parameter.Deft]) * 100f;

            // Use Item
            Progress.UseItems(data.Materials);
            // Get Item
            Progress.AddItem(data.Result.Item.Id, data.Result.Count + (bonus ? 1 : 0));

            CheckAction(levelData);
        }

        public void BuyMonster(MonsterData data) {
            if(!Progress.CheckItemsEnough(data.Cost))
                return;

            Progress.UseItems(data.Cost);
            Progress.AddMonster(data.Id, 1);
        }

        private void CheckAction(ActionLevelData data) {
            Progress.ConsumeAP(data.AP);
            Progress.GetParameters(data.RewardParameters);
        }
        #endregion

        #region SceneManagement
        private void LoadData() {
            var stageId = DataManager.Instance.Stages[Progress.Stage];

            CurrentStage = DataManager.Instance.LoadStageData(stageId);

            var stageSequencePath = CurrentStage.Datas[Progress.StageSequence];

            CurrentStageSequence = DataManager.Instance.LoadStageSequenceData(stageSequencePath);

            Debug.Log(stageSequencePath);
        }

        private async UniTask LoadDataAsync() {
            var stageId = DataManager.Instance.Stages[Progress.Stage];

            CurrentStage = await DataManager.Instance.LoadStageDataAsync(stageId);

            var stageSequenceId = CurrentStage.Datas[Progress.StageSequence];

            CurrentStageSequence = await DataManager.Instance.LoadStageSequenceDataAsync(stageSequenceId);
        }

        private void LoadScene() {
            if(Progress.Stage == DataManager.Instance.Stages.Count) {
                LoadEndingScene();
                return;
            }

            if(CurrentStageSequence is DailyRoutineData) {
                // daily routine
                if(Progress.DDay > 0) {
                    LoadMainScene();
                } else {
                    if((CurrentStageSequence as DailyRoutineData).Battle == null) {
                        StageSequenceEnd();
                        return;
                    } else {
                        LoadBattleScene();
                    }
                }
            } else if(CurrentStageSequence is DialogueData) {
                // dialogue
                LoadDialogueScene();
            } else {
                throw new Exception(string.Format("Stage Sequence is Undefined Data type"));
            }
        }

        public void LoadTitleScene() {
            string json = PlayerPrefs.GetString(GameProgressKey, "");
            if(json == "") {
                this.Progress = new GameProgress();
            } else {
                this.Progress = GameProgress.FromJson(json);
            }

            LoadData();

            SceneManager.LoadScene(titleScene);
        }

        public void LoadMainScene() {
            SceneManager.LoadScene(mainScene);
        }

        public void LoadBattleScene() {
            SceneManager.LoadScene(battleScene);
        }

        public void LoadDialogueScene() {
            SceneManager.LoadScene(dialogueScene);
        }

        public void LoadEndingScene() {
            SceneManager.LoadScene(endingScene);
        }
        #endregion

        [Button]
        private void DeletePrefsKey(string key) {
            PlayerPrefs.DeleteKey(key);
        }
    }
}