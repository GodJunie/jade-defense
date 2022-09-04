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

        [SerializeField]
        private ParameterData parameterData;
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
            Progress.SetAp(MaxAp);
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
            var items = new Dictionary<int, int>();

            for(int i = 0; i < FarmingCount; i++) {
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

            // Use Item
            Progress.UseItems(data.Materials);
            // Get Item
            Progress.AddItem(data.Result.Item.Id, data.Result.Count + (CraftingBonus ? 1 : 0));

            CheckAction(levelData);
        }

        public void BuyMonster(MonsterData data) {
            if(!Progress.CheckItemsEnough(data.Cost))
                return;

            Progress.UseItems(data.Cost);
            Progress.AddMonster(data.Id, 1);
        }

        private void CheckAction(ActionLevelData data) {
            Progress.ConsumeAP(data.AP * (1 - ApDiscountRate));
            Progress.GetParameters(data.RewardParameters);
        }

        public bool CheckApEnough(float ap) {
            ap *= (1 - ApDiscountRate);

            return this.Progress.AP >= ap;
        }

        public void BuyOnTrade<T>(T data) where T : IDataID, ISale {
            if(!Progress.Trades.ContainsKey(data.Id)) {
                throw new Exception(string.Format("there is no item in trade list, id: {0}", data.Id));
            }

            int price = data.BuyPrice;

            price = Mathf.FloorToInt(price * (1 - TradeDiscountRate));

            Progress.UseItem(GameConsts.GOLD_ID, price);

            if(data is ItemData)
                Progress.AddItem(data.Id, 1);
            else if(data is MonsterData)
                Progress.AddMonster(data.Id, 1);
            else
                throw new Exception("Data is not ItemData nor MonsterData");

            Progress.Trades[data.Id]--;
        }

        public void BuyOnTrade(ScriptableObject data) {
            if(data is ItemData)
                BuyOnTrade(data as ItemData);
            else if(data is MonsterData)
                BuyOnTrade(data as MonsterData);
            else
                throw new Exception("Data is not ItemData nor MonsterData");
        }

        public void SellOnTrade<T>(T data) where T : IDataID, ISale {
            if(data is ItemData)
                Progress.UseItem(data.Id, 1);
            else if(data is MonsterData)
                Progress.UseMonster(data.Id, 1);
            else
                throw new Exception("Data is not ItemData nor MonsterData");

            Progress.AddItem(GameConsts.GOLD_ID, data.SellPrice);
        }

        public void SellOnTrade(ScriptableObject data) {
            if(data is ItemData)
                SellOnTrade(data as ItemData);
            else if(data is MonsterData)
                SellOnTrade(data as MonsterData);
            else
                throw new Exception("Data is not ItemData nor MonsterData");
        }

        public bool CheckCanBuyOnTrade<T>(T data) where T : IDataID, ISale{
            var price = Mathf.FloorToInt(data.BuyPrice * (1 - TradeDiscountRate));

            return Progress.Gold >= price && Progress.Trades.ContainsKey(data.Id) && Progress.Trades[data.Id] > 0;
        }

        public bool CheckCanBuyOnTrade(ScriptableObject data) {
            if(data is ItemData)
                return CheckCanBuyOnTrade(data as ItemData);
            else if(data is MonsterData)
                return CheckCanBuyOnTrade(data as MonsterData);
            else
                throw new Exception("Data is not ItemData nor MonsterData");
        }
        #endregion

            #region Parameter Data
        public float ParameterMaxValue {
            get {
                return parameterData.ParameterMaxValue;
            }
        }

        public float MaxAp {
            get {
                return parameterData.GetMaxAp(Progress);
            }
        }

        public float ApDiscountRate {
            get {
                return parameterData.GetApDiscountRate(Progress);
            }
        }

        public float TradeDiscountRate {
            get {
                return parameterData.GetTradeDiscountRate(Progress);
            }
        }

        public int FarmingCount {
            get {
                return parameterData.GetFarmingCount(Progress);
            }
        }

        public float CraftingBonusRate {
            get {
                return parameterData.GetCraftingBonusRate(Progress);
            }
        }

        public bool CraftingBonus {
            get {
                return UnityEngine.Random.Range(0f, 100f) < CraftingBonusRate * 100f;
            }
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