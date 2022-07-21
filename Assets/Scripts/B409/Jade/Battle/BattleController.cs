using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace B409.Jade.Battle {
    using Data;

    public class BattleController : MonoBehaviour {
        // �׽�Ʈ�� ���ؼ� Serialize �߰� ���� �ΰ��ӿ����� Stage �����Ϳ��� Battle Data �޾ƿ���, GameProgress ���� MonsterDatas �޾ƿ���
        [SerializeField]
        [InlineEditor]
        private BattleData data;
        [SerializeField]
        private List<UnitData> monsterDatas;

        [SerializeField]
        private Transform monsterSpawnPoint;
        [SerializeField]
        private Transform enemySpawnPoint;

        private bool isPlaying;

        private List<UnitController> monsters = new List<UnitController>();
        private List<UnitController> enemies = new List<UnitController>();

        // Start is called before the first frame update
        void Start() {
            GameStart();
        }

        // Update is called once per frame
        void Update() {
            
        }

        public void GameStart() {
            GenerateMonsters();
            GenerateEnemies();
        }

        private async void GenerateMonsters() {
            foreach(var monsterData in monsterDatas) {
                await GenerateUnit(monsterData, true);
            }
        }

        private async void GenerateEnemies() {
            foreach(var enemyData in data.Enemies) {
                await GenerateUnit(enemyData, false);
            }
        }

        private async UniTask GenerateUnit(UnitData data, bool isPlayer) {
            await UniTask.Delay(TimeSpan.FromSeconds(data.Status.Cooltime));
            if(isPlayer) {
                var unit = Instantiate(data.Prefab, monsterSpawnPoint.position, monsterSpawnPoint.rotation);
                this.monsters.Add(unit);
                unit.Init(data, isPlayer);
            } else {
                var unit = Instantiate(data.Prefab, enemySpawnPoint.position, enemySpawnPoint.rotation);
                this.enemies.Add(unit);
                unit.Init(data, isPlayer);
            }
        }
    }
}