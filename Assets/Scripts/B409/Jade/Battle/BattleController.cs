using System;
using System.Collections;
using System.Collections.Generic;
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
        private GameObject panelSetParty;
        [TitleGroup("Party")]
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private MonsterOwnedSlot monsterSlotPrefab;
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private Transform monsterSlotContainer;
        [FoldoutGroup("Party/Owned")]
        [SerializeField]
        private ScrollRect monsterScrollRect;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject panelSelectedMonster;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private Image imageSelectedMonster;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterName;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterCooltime;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterRange;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterHp;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterSpeed;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private TMP_Text textSelectedMonsterDescription;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject buttonMonsterIn;
        [FoldoutGroup("Party/Info")]
        [SerializeField]
        private GameObject buttonMonsterOut;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<Image> imagesPartyMonster;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<GameObject> statusesPartyMonster;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<TMP_Text> textsPartyCooltime;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<TMP_Text> textsPartyRange;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<TMP_Text> textsPartyHp;
        [FoldoutGroup("Party/Party")]
        [SerializeField]
        private List<TMP_Text> textsPartySpeed;


        [TitleGroup("Map")]
        [SerializeField]
        private float mapSize = 5f;
        [TitleGroup("Map")]
        [SerializeField]
        private float spawnPosPad = 2f;



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


        private Transform cameraTransform;
        private float cameraBoundX;
        private float minimapRatio;

        private bool isPlaying = false;

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
                var progress = GameManager.Instance.Progress;
                var stage = DataManager.Instance.Stages[progress.Stage];
                var stageSequence = stage.Datas[progress.StageSequence];

                Instantiate(stage.InGameBackground);

                if(!(stageSequence is DailyRoutineData)) {
                    return;
                }
                this.data = (stageSequence as DailyRoutineData).Battle;
            }

            this.enemyDatas = data.Enemies;
            
            if(isTest) {
                GameStart();
            } else {
                SetMakePartyUI();
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
            }
        }

        #region Party
        private MonsterData selectedMonsterData;
        private int selectedMonsterIndex;
        private Dictionary<int, MonsterOwnedSlot> monsterSlotPool = new Dictionary<int, MonsterOwnedSlot>();

        private void SetMakePartyUI() {
            panelSetParty.SetActive(true);
            panelSelectedMonster.SetActive(false);
            this.monsterDatas.Clear();

            var progress = GameManager.Instance.Progress;

            foreach(var monster in progress.Monsters) {
                var slot = Instantiate(monsterSlotPrefab, monsterSlotContainer);
                int id = monster.Key;
                int count = monster.Value;
                var data = DataManager.Instance.Monsters.Find(e => e.Id == id);
                slot.Init(data, count, monsterScrollRect, () => {
                    ShowMonsterInfo(data, true);
                });
                this.monsterSlotPool.Add(id, slot);
            }

            SetInPartyMonsterUI();
        }

        private void ShowMonsterInfo(MonsterData data, bool monsterIn) {
            this.selectedMonsterData = data;

            panelSelectedMonster.SetActive(true);

            imageSelectedMonster.sprite = data.Icon;
            textSelectedMonsterName.text = data.Name;
            // status
            textSelectedMonsterCooltime.text = data.Status.Cooltime.ToString("0.#");
            textSelectedMonsterRange.text = data.Status.Range.ToString("0.#");
            textSelectedMonsterHp.text = data.Status.Hp.ToString("0");
            textSelectedMonsterSpeed.text = data.Status.MoveSpeed.ToString("0.#");
            textSelectedMonsterDescription.text = data.Status.GetAttackDescriptionString();

            buttonMonsterIn.SetActive(monsterIn);
            buttonMonsterOut.SetActive(!monsterIn);
        }

        public void MonsterInParty(bool monsterIn) {
            if(monsterIn) {
                if(this.monsterDatas.Count == 5)
                    return;

                int id = selectedMonsterData.Id;

                this.monsterDatas.Add(selectedMonsterData);

                var progress = GameManager.Instance.Progress;
                progress.UseMonster(id, 1);

                if(progress.Monsters.ContainsKey(id)) {
                    this.monsterSlotPool[id].Init(selectedMonsterData.Icon, progress.Monsters[id]);
                } else {
                    this.monsterSlotPool[id].gameObject.SetActive(false);
                }
            } else {
                if(selectedMonsterIndex < this.monsterDatas.Count) {
                    var monsterData = this.monsterDatas[selectedMonsterIndex];

                    this.monsterDatas.RemoveAt(selectedMonsterIndex);

                    var progress = GameManager.Instance.Progress;
                    progress.AddMonster(monsterData.Id, 1);

                    var slot = monsterSlotPool[monsterData.Id];
                    slot.gameObject.SetActive(true);
                    slot.Init(monsterData.Icon, progress.Monsters[monsterData.Id]);
                }
            }

            panelSelectedMonster.SetActive(false);
            selectedMonsterData = null;

            SetInPartyMonsterUI();
        }

        private void SetInPartyMonsterUI() {
            for(int i = 0; i < 5; i++) {
                var image = imagesPartyMonster[i];
                var status = statusesPartyMonster[i];
                var cooltime = textsPartyCooltime[i];
                var range = textsPartyRange[i];
                var hp = textsPartyHp[i];
                var speed = textsPartySpeed[i];

                if(i < monsterDatas.Count) {
                    var data = monsterDatas[i];
                    image.gameObject.SetActive(true);
                    image.sprite = data.Icon;
                    status.SetActive(true);
                    cooltime.text = data.Status.Cooltime.ToString("0.#");
                    range.text = data.Status.Range.ToString("0.#");
                    hp.text = data.Status.Hp.ToString("0");
                    speed.text = data.Status.MoveSpeed.ToString("0.#");
                } else {
                    image.gameObject.SetActive(false);
                    status.SetActive(false);
                }
            }
        }

        public void SelectMonsterInParty(int index) {
            if(index < this.monsterDatas.Count) {
                this.selectedMonsterIndex = index;

                ShowMonsterInfo(this.monsterDatas[index] as MonsterData, false);
            }
        }

        public void MoveUp(int index) {
            if(index > 0 && index < this.monsterDatas.Count) {
                if(this.selectedMonsterData != null) {
                    if(index == this.selectedMonsterIndex)
                        this.selectedMonsterIndex = index - 1;
                    else if(this.selectedMonsterIndex == index - 1)
                        this.selectedMonsterIndex = index;
                }

                var prev = this.monsterDatas[index - 1];
                var data = this.monsterDatas[index];
                this.monsterDatas[index - 1] = data;
                this.monsterDatas[index] = prev;


                SetInPartyMonsterUI();
            }
        }

        public void MoveDown(int index) {
            if(index < this.monsterDatas.Count - 1) {
                if(this.selectedMonsterData != null) {
                    if(index == this.selectedMonsterIndex)
                        this.selectedMonsterIndex = index + 1;
                    else if(this.selectedMonsterIndex == index + 1)
                        this.selectedMonsterIndex = index;
                }

                var data = this.monsterDatas[index];
                var next = this.monsterDatas[index + 1];
                this.monsterDatas[index] = next;
                this.monsterDatas[index + 1] = data;

                SetInPartyMonsterUI();
            }
        }
        #endregion


        #region Game
        public void GameStart() {
            panelSetParty.SetActive(false);

            SetMonsterWait();
            SetEnemyWait();

            isPlaying = true;

            this.monsterCount = this.monsterDatas.Count;
            this.enemyCount = this.enemyDatas.Count;

            SetMonsterCountUI();
            SetEnemyCountUI();

            GenerateMonsters();
            GenerateEnemies();
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
            var posX = isPlayer ? -1f : 1f;
            posX *= (mapSize + spawnPosPad);
            var pos = new Vector3(posX, -1.15f, 0f);
            pos += new Vector3(0f, 1f, 1f) * UnityEngine.Random.Range(-0.3f, 0.3f);

            var unit = Instantiate(data.Prefab, pos, Quaternion.identity);
            unit.Init(data, isPlayer, this.mapSize);

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
            Debug.Log("Clear!");
            foreach(var monster in this.monsters) {
                GameManager.Instance.Progress.AddMonster(monster.Data.Id, 1);
                monster.OnStop();
            }
            GameManager.Instance.StageSequenceEnd();
        }

        private void StageFailed() {
            Debug.Log("Failed!");
            foreach(var enemy in this.enemies) {
                enemy.OnStop();
            }
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
    }
}