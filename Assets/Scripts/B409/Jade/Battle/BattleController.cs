using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;

namespace B409.Jade.Battle {
    using Data;
    using Game;
    using UI;

    public class BattleController : MonoBehaviour {
        // 테스트를 위해서 Serialize 했고 실제 인게임에서는 Stage 데이터에서 Battle Data 받아오고, GameProgress 에서 MonsterDatas 받아오기
        [TitleGroup("Test")]
        [SerializeField]
        private bool isTest = false;
        [TitleGroup("Test")]
        [SerializeField]
        [InlineEditor]
        private BattleData data;
        [TitleGroup("Test")]
        [SerializeField]
        private List<UnitData> monsterDatas = new List<UnitData>();

        [TitleGroup("General")]
        [SerializeField]
        private Image imageFade;


        [TitleGroup("Party")]
        [SerializeField]
        private PartyPanel panelSetParty;


        [TitleGroup("Map")]
        [SerializeField]
        private float mapSize = 5f;
        [TitleGroup("Map")]
        [SerializeField]
        private float spawnPosPad = 1f;
        [TitleGroup("Map")]
        [SerializeField]
        private float mapPad = 2f;


        [TitleGroup("Camera")]
        [SerializeField]
        private float cameraScrollSpeed = 0.5f;



        [TitleGroup("Minimap")]
        [SerializeField]
        private Transform pinContainer;
        [TitleGroup("Minimap")]
        [SerializeField]
        private Pin pinPrefab;
        [TitleGroup("Minimap")]
        [SerializeField]
        private RectTransform minimapRect;
        [TitleGroup("Minimap")]
        [SerializeField]
        private RectTransform minimapFrameRect;


        [TitleGroup("UI")]
        [HorizontalGroup("UI/group", .5f)]
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private List<Image> imageWaitMonsters;
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private List<TMP_Text> textCooltimeWaitMonsters;
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private Image imageCurrentMonster;
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private Image imageCooltimeMonster;
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private TMP_Text textCooltimeMonster;
        [BoxGroup("UI/group/Monster")]
        [SerializeField]
        private TMP_Text textMonsterCount;

        [HorizontalGroup("UI/group", .5f)]
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private List<Image> imageWaitEnemies;
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private List<TMP_Text> textCooltimeWaitEnemies;
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private Image imageCurrentEnemy;
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private Image imageCooltimeEnemy;
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private TMP_Text textCooltimeEnemy;
        [BoxGroup("UI/group/Enemy")]
        [SerializeField]
        private TMP_Text textEnemyCount;

        [TitleGroup("UI")]
        [BoxGroup("UI/StageClear")]
        [SerializeField]
        private GameObject panelStageFailed;
        [BoxGroup("UI/StageClear")]
        [SerializeField]
        private ItemSlot itemSlotPrefab;
        [BoxGroup("UI/StageClear")]
        [SerializeField]
        private Transform itemSlotContainer;
        [BoxGroup("UI/StageClear")]
        [SerializeField]
        private ResultMonsterSlot resultMonsterSlotPrefab;
        [BoxGroup("UI/StageClear")]
        [SerializeField]
        private Transform resultMonsterSlotContainer;


        [TitleGroup("UI")]
        [SerializeField]
        private GameObject panelStageClear;
        [TitleGroup("UI")]
        [SerializeField]
        private GameObject panelPause;


        [TitleGroup("Sound")]
        [SerializeField]
        private AudioClip battleRobbyBgm;
        [TitleGroup("Sound")]
        [SerializeField]
        private AudioClip clearBgm;
        [TitleGroup("Sound")]
        [SerializeField]
        private AudioClip failedBgm;



        private Transform cameraTransform;
        private float cameraBoundX;
        private float minimapRatio;

        private bool isPlaying = false;
        private bool isPaused = false;

        private List<UnitController> monsters = new List<UnitController>();
        private List<UnitController> enemies = new List<UnitController>();


        private List<UnitData> enemyDatas = new List<UnitData>();


        float monsterTimer = 0f;
        float enemyTimer = 0f;
        private int monsterGenIndex = 0;
        private int enemyGenIndex = 0;

        private UnitData waitMonster => monsterGenIndex < monsterDatas.Count ? monsterDatas[monsterGenIndex] : null;
        private UnitData waitEnemy => enemyGenIndex < enemyDatas.Count ? enemyDatas[enemyGenIndex] : null;

        private int monsterCount;
        private int enemyCount;


        // Start is called before the first frame update
        void Start() {
            imageFade.gameObject.SetActive(true);
            imageFade.color = Color.white;
            imageFade.DOFade(0f, 1f);

            InitCamera();

            if(!isTest) {
                var stageSequence = GameManager.Instance.CurrentStageSequence;

                if(!(stageSequence is DailyRoutineData)) {
                    return;
                }

                this.data = (stageSequence as DailyRoutineData).Battle;
            }

            Instantiate(this.data.Background);

            this.enemyDatas = data.Enemies;

            SoundManager.Instance.PlayBgm(battleRobbyBgm);

            if(isTest) {
                GameStart();
            } else {
                panelSetParty.Open();
            }
        }

        // Update is called once per frame
        void Update() {
            if(isPlaying) {
                CameraScroll();

                if(monsterGenIndex < monsterDatas.Count) {
                    GenerateMonsters();
                }

                if(enemyGenIndex < enemyDatas.Count) {
                    GenerateEnemies();
                }

                if(Input.GetKeyDown(KeyCode.Escape)) {
                    if(isPaused) {
                        Resume();
                    } else {
                        Pause();
                    }
                }
            }
        }

        #region Game
        public void InitMonsterData(List<UnitData> monsterDatas) {
            this.monsterDatas = monsterDatas;
            GameStart();
        }
        
        private void GameStart() {
            SetMonsterWait();
            SetEnemyWait();

            isPlaying = true;

            this.monsterCount = this.monsterDatas.Count;
            this.enemyCount = this.enemyDatas.Count;

            SetMonsterCountUI();
            SetEnemyCountUI();

            GenerateMonsters();
            GenerateEnemies();

            SoundManager.Instance.PlayBgm(data.Bgm);
        }

        private void SetMonsterCountUI() {
            this.textMonsterCount.text = this.monsterCount.ToString();
        }

        private void SetEnemyCountUI() {
            this.textEnemyCount.text = this.enemyCount.ToString();
        }

        private void GenerateMonsters() {
            monsterTimer -= Time.deltaTime;

            if(monsterTimer <= 0f) {
                GenerateUnit(waitMonster, true);
                monsterGenIndex++;
                SetMonsterWait();
                return;
            }

            this.imageCooltimeMonster.fillAmount = monsterTimer / waitMonster.Status.Cooltime;
            this.textCooltimeMonster.text = monsterTimer.ToString("0");
        }

        private void GenerateEnemies() {
            enemyTimer -= Time.deltaTime;

            if(enemyTimer <= 0f) {
                GenerateUnit(waitEnemy, false);
                enemyGenIndex++;
                SetEnemyWait();
                return;
            }

            this.imageCooltimeEnemy.fillAmount = enemyTimer / waitEnemy.Status.Cooltime;
            this.textCooltimeEnemy.text = enemyTimer.ToString("0");
        }

        private void SetMonsterWait() {
            if(monsterGenIndex < monsterDatas.Count) {
                this.monsterTimer = waitMonster.Status.Cooltime;

                this.imageCurrentMonster.sprite = waitMonster.Icon;
            } else {
                this.imageCooltimeMonster.fillAmount = 1f;
                this.textCooltimeMonster.text = "";
                this.imageCurrentMonster.gameObject.SetActive(false);
            }

            for(int i = 0; i < 4; i++) {
                var image = imageWaitMonsters[i];
                var text = textCooltimeWaitMonsters[i];
                int index = monsterGenIndex + 1 + i;
                if(index < monsterDatas.Count) {
                    var monster = monsterDatas[index];
                    image.gameObject.SetActive(true);
                    text.text = monster.Status.Cooltime.ToString("0");
                    image.sprite = monster.Icon;
                } else {
                    image.gameObject.SetActive(false);
                    text.text = "";
                }
            }
        }

        private void SetEnemyWait() {
            if(enemyGenIndex < enemyDatas.Count) {
                this.enemyTimer = waitEnemy.Status.Cooltime;

                this.imageCurrentEnemy.sprite = waitEnemy.Icon;
            } else {
                this.imageCooltimeEnemy.fillAmount = 1f;
                this.textCooltimeEnemy.text = "";
                this.imageCurrentEnemy.gameObject.SetActive(false);
            }

            for(int i = 0; i < 4; i++) {
                var image = imageWaitEnemies[i];
                var text = textCooltimeWaitEnemies[i];
                int index = enemyGenIndex + 1 + i;
                if(index < enemyDatas.Count) {
                    var enemy = enemyDatas[index];
                    image.gameObject.SetActive(true);
                    text.text = enemy.Status.Cooltime.ToString("0");
                    image.sprite = enemy.Icon;
                } else {
                    image.gameObject.SetActive(false);
                    text.text = "";
                }
            }
        }

        private void GenerateUnit(UnitData data, bool isPlayer) {
            float posX = mapSize;

            if(data.Status.MoveSpeed == 0) {
                posX -= mapPad;
            } else {
                posX += spawnPosPad;
            }
            posX *= isPlayer ? -1f : 1f;

            var pos = new Vector3(posX, -1.15f, 0f);
            pos += new Vector3(0f, 1f, 1f) * UnityEngine.Random.Range(-0.3f, 0.3f);

            var unit = Instantiate(data.Prefab, pos, Quaternion.identity);
            unit.Init(data, isPlayer, this.mapSize - mapPad);

            unit.OnDead += () => {
                if(isPlayer) {
                    this.monsterCount--;
                    SetMonsterCountUI();
                    if(this.monsterCount == 0) {
                        StageFailed();
                    }
                } else {
                    this.enemyCount--;
                    SetEnemyCountUI();
                    if(this.enemyCount == 0) {
                        StageClear();
                    }
                }
            };

            if(isPlayer) {
                this.monsters.Add(unit);
            } else {
                this.enemies.Add(unit);
            }

            var pin = Instantiate(pinPrefab, pinContainer);
            pin.gameObject.SetActive(false);
            pin.Init(unit, this.minimapRatio, isPlayer);
        }

        private void StageClear() {
            var progress = GameManager.Instance.Progress;

            SoundManager.Instance.PlayBgm(clearBgm);

            for(int i = 0; i < monsterDatas.Count; i++) {
                var monsterData = monsterDatas[i];
                bool alive = true;
                if(monsters.Count > i) {
                    var monster = monsters[i];
                    if(monster == null)
                        alive = false;
                    else
                        monster.OnStop();
                }

                var slot = Instantiate(resultMonsterSlotPrefab, resultMonsterSlotContainer);
                slot.Init(monsterData.Icon, !alive);

                if(alive)
                    progress.AddMonster(monsterData.Id, 1);
            }

            foreach(var reward in this.data.Rewards) {
                var item = reward.Item;
                var count = reward.Count;

                var slot = Instantiate(itemSlotPrefab, itemSlotContainer);
                slot.Init(item, count);

                progress.AddItem(item.Id, count);
            }

            panelStageClear.SetActive(true);

            for(int i = 0; i < panelStageClear.transform.childCount; i++) {
                var child = panelStageClear.transform.GetChild(i);
                child.localScale = Vector3.one * 1.2f;
                child.DOScale(1f, 1f);
            }

            foreach(var g in panelStageClear.GetComponentsInChildren<Graphic>()) {
                var color = g.color;
                var alpha = color.a;
                color.a = 0f;
                g.color = color;
                g.DOFade(alpha, 1f);
            }
        }

        private void StageFailed() {
            SoundManager.Instance.PlayBgm(failedBgm);

            foreach(var enemy in this.enemies) {
                enemy.OnStop();
            }

            panelStageFailed.SetActive(true);

            for(int i = 0; i < panelStageFailed.transform.childCount; i++) {
                var child = panelStageFailed.transform.GetChild(i);
                child.localScale = Vector3.one * 1.2f;
                child.DOScale(1f, 1f);
            }

            foreach(var g in panelStageFailed.GetComponentsInChildren<Graphic>()) {
                var color = g.color;
                var alpha = color.a;
                color.a = 0f;
                g.color = color;
                g.DOFade(alpha, 1f);
            }
        }

        public async void NextStage() {
            imageFade.gameObject.SetActive(true);
            await imageFade.DOFade(1f, 1f);
            GameManager.Instance.StageSequenceEnd();
        }

        public async void GoToMain() {
            imageFade.gameObject.SetActive(true);
            await imageFade.DOFade(1f, 1f);
            GameManager.Instance.Retry();
        }
        #endregion

        #region Screen and Minimap
        private int pointerId = 0;
        private float minimapPosX;
        private float frameSizeX;
        private float minimapSizeX;

        private void InitCamera() {
            var cam = Camera.main;
            cameraTransform = cam.transform;
            var camPos = cameraTransform.position;
            cameraBoundX = mapSize - cam.orthographicSize * cam.aspect;
            //cameraTransform.position = new Vector3(-cameraBoundX, camPos.y, camPos.z);

            var cameraWidth = cam.orthographicSize * cam.aspect * 2;
            frameSizeX = minimapFrameRect.sizeDelta.x;

            minimapRatio = frameSizeX / cameraWidth;
            minimapSizeX = minimapRatio * mapSize * 2 + 14f;
            this.minimapRect.sizeDelta = new Vector2(minimapSizeX, this.minimapRect.sizeDelta.y);

            minimapPosX = minimapRect.anchoredPosition.x + minimapSizeX / 2;

            Debug.Log(minimapRect.localPosition);
        }

        private void CameraScroll() {
            var mousePos = Input.mousePosition;

            if(pointerId != 0) {
                MoveMinimap(mousePos.x);
                return;
            }

            var deltaX = 0f;

            if(mousePos.x < Screen.width * 0.05f) {
                deltaX = -1f;
            } else if(mousePos.x > Screen.width * (1 - 0.05f)) {
                deltaX = 1f;
            } else {
                return;
            }

            deltaX *= cameraScrollSpeed * Time.deltaTime;

            var pos = cameraTransform.position;

            pos.x = Mathf.Clamp(pos.x + deltaX, -cameraBoundX, cameraBoundX);

            cameraTransform.position = pos;

            this.minimapFrameRect.anchoredPosition = new Vector2(cameraTransform.position.x * minimapRatio, 0f);
        }

        public void OnPointerDown(BaseEventData data) {
            var pointerData = data as PointerEventData;

            if(pointerData.button != PointerEventData.InputButton.Left)
                return;

            if(pointerId != 0) {
                return;
            }

            pointerId = pointerData.pointerId;

            Debug.Log(pointerData);

            MoveMinimap(pointerData.pointerCurrentRaycast.screenPosition.x);
        }

        public void OnPointerUp(BaseEventData data) {
            var pointerData = data as PointerEventData;

            if(pointerData.button != PointerEventData.InputButton.Left)
                return;

            if(pointerId == pointerData.pointerId) {
                pointerId = 0;
            }
        }


        private void MoveMinimap(float x) {
            x = x * 1920f / Screen.width;
            x -= minimapPosX;

            var bound = (minimapSizeX - frameSizeX) / 2 - 7f;
            x = Mathf.Clamp(x, -bound, bound);

            minimapFrameRect.anchoredPosition = new Vector2(x, 0f);

            var cPos = cameraTransform.position;
            cPos.x = x / minimapRatio;
            cameraTransform.position = cPos;
        }
        #endregion

        #region Pause
        public void Pause() {
            Time.timeScale = 0f;
            panelPause.SetActive(true);
            isPaused = true;
        }

        public void Resume() {
            Time.timeScale = 1f;
            panelPause.SetActive(false);
            isPaused = false;
        }
        #endregion
    }
}