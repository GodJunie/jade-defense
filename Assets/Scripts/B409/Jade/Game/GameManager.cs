using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace B409.Jade.Game {
    using Data;

    public class GameManager : SingletonBehaviour<GameManager> {
        #region Consts 
        private const string GameProgressKey = "GameProgress";
        #endregion

        #region SerializedField
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
        #endregion

        #region Mono
        protected override void Awake() {
            base.Awake();
            string json = PlayerPrefs.GetString(GameProgressKey, "");
            if(json == "") {
                this.Progress = new GameProgress();
            } else {
                this.Progress = GameProgress.FromJson(json);
            }
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

        public void Save() {
            string json = this.Progress.ToJson();
            PlayerPrefs.SetString(GameProgressKey, json);
        }
        #endregion

        #region Game
        public void TurnStart() {
            Progress.TurnStart();
        }

        public void TurnEnd() {
            var stage = DataManager.Instance.Stages[Progress.Stage];
            var stageSequence = stage.Datas[Progress.StageSequence];

            if(!(stageSequence is DailyRoutineData))
                return;

            Progress.TurnEnd();

            Save();
            LoadScene();
        }

        public void StageSequenceEnd() {
            var stage = DataManager.Instance.Stages[Progress.Stage];

            Progress.NextStageSequence();

            if(Progress.StageSequence == stage.Datas.Count) {
                // Stage Clear!
                Progress.NextStage();
            }

            StageSequenceStart();
            Save();
            LoadScene();
        }

        private void StageSequenceStart() {
            if(Progress.Stage == DataManager.Instance.Stages.Count) {
                return;
            }

            var stage = DataManager.Instance.Stages[Progress.Stage];
            var data = stage.Datas[Progress.StageSequence];

            if(!(data is DailyRoutineData))
                return;

            Progress.DailyRoutineStart(data as DailyRoutineData);
        }
        #endregion

        #region Farming
        public void Farming(FarmingLevelData data) {
            int count = 5;
            var items = new Dictionary<int, int>();

            for(int i = 0; i < count; i++) {
                Debug.Log("get");

                var sum = data.Datas.Sum(e => e.Rate);
                var rand = UnityEngine.Random.Range(0f, sum);

                foreach(var rateData in data.Datas) {
                    if(rand < rateData.Rate) {
                        var item = rateData.Item;
                        if(items.ContainsKey(item.Id))
                            items[item.Id] += 1;
                        else
                            items.Add(item.Id, 1);
                        break;
                    }

                    rand -= rateData.Rate;
                }
            }

            Progress.AddItems(items);
            CheckAction(data);

            Debug.Log(Progress.ToJson());
        }

        public void Making(ActionLevelData levelData, RecipeData data) {
            // Use Item
            foreach(var mat in data.Materials) {
                Progress.UseItem(mat.Item.Id, mat.Count);
            }
            // Get Item
            Progress.AddItem(data.Result.Item.Id, data.Result.Count);

            CheckAction(levelData);
        }

        private void CheckAction(ActionLevelData data) {
            Progress.ConsumeAP(data.AP);
            Progress.GetParameters(data.RewardParameters);
        }
        #endregion

        #region SceneManagement
        private void LoadScene() {
            if(Progress.Stage == DataManager.Instance.Stages.Count) {
                LoadEndingScene();
                return;
            }

            var stage = DataManager.Instance.Stages[Progress.Stage];
            var stageSequence = stage.Datas[Progress.StageSequence];

            if(stageSequence is DailyRoutineData) {
                // daily routine
                if(Progress.DDay > 0) {
                    LoadMainScene();
                } else {
                    LoadBattleScene();
                }
            } else if(stageSequence is DialogueData) {
                // dialogue
                LoadDialogueScene();
            } else {
                throw new Exception(string.Format("Stage Sequence is Undefined Data type"));
            }
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
    }
}