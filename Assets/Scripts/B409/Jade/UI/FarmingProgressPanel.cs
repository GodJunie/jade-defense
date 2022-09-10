using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;



namespace B409.Jade.UI {
    using Data;
    using Game;

    public class FarmingProgressPanel : MonoBehaviour {
        [TitleGroup("Settings")]
        [SerializeField]
        private MainScreen mainScreen;
        [TitleGroup("Settings")]
        [SerializeField]
        private float progressDuration = 3f;
        [TitleGroup("Settings")]
        [SerializeField]
        private float progressInterval = 1f;


        [TitleGroup("UI")]
        [SerializeField]
        private GameObject buttonQuit;

        [TitleGroup("UI")]
        [FoldoutGroup("UI/Progress")]
        [SerializeField]
        private RectTransform panelProgress;
        [FoldoutGroup("UI/Progress")]
        [SerializeField]
        private Slider sliderProgress;
        [FoldoutGroup("UI/Progress")]
        [SerializeField]
        private Image imageProgressIcon;
        [FoldoutGroup("UI/Progress")]
        [SerializeField]
        private TMP_Text textProgressLog;

        [FoldoutGroup("UI/Result")]
        [SerializeField]
        private RectTransform panelResult;
        [FoldoutGroup("UI/Result")]
        [SerializeField]
        private Transform slotContainer;
        [FoldoutGroup("UI/Result")]
        [SerializeField]
        private ItemSlot slotPrefab;
        [FoldoutGroup("UI/Result")]
        [SerializeField]
        private BeforeAfterText textAp;


        private List<ItemSlot> slotPool = new List<ItemSlot>();

        public async void Open(FarmingLevelData data) {
            var progress = GameManager.Instance.Progress;

            gameObject.SetActive(true);
            panelProgress.gameObject.SetActive(true);
            panelResult.gameObject.SetActive(false);

            panelProgress.anchoredPosition = Vector2.zero;

            var progressTasks = new List<UniTask>();

            this.sliderProgress.value = 0f;
            progressTasks.Add(this.sliderProgress.DOValue(1f, progressDuration).ToUniTask());

            progressTasks.Add(ChangeProgressSprites(data.Datas.Where(e => e.Item != null).Select(e => e.Item.Icon).ToList().Shuffle(), progressInterval, progressDuration));

            progressTasks.Add(ProgressText(data.ProgressLogs.GetRandomItem(), progressDuration));

            await UniTask.WhenAny(progressTasks);

            panelResult.gameObject.SetActive(true);
            panelResult.anchoredPosition = new Vector2(1000f, 0f);

            float apBefore = progress.AP;

            var result = GameManager.Instance.Farming(data);
            mainScreen.Init();

            float apAfter = progress.AP;

            foreach(var slot in slotPool) {
                slot.gameObject.SetActive(false);
            }

            foreach(var pair in result) {
                int id = pair.Key;
                int count = pair.Value;
                var slot = GetItemSlot();
                slot.Init(id, count);
            }

            await UniTask.WhenAll(
                panelProgress.transform.DOLocalMoveX(-1000f, .5f).ToUniTask(),
                panelResult.transform.DOLocalMoveX(0f, .5f).ToUniTask()
            );

            panelProgress.gameObject.SetActive(false);

            await textAp.Show(apBefore, apAfter, .5f);

            buttonQuit.SetActive(true);
        }

        private async UniTask ChangeProgressSprites(List<Sprite> sprites, float interval, float duration) {
            int index = 0;
            while(duration > 0f) {
                this.imageProgressIcon.sprite = sprites[index];
                await UniTask.Delay(TimeSpan.FromSeconds(interval));
                duration -= interval;
                index++;
                if(index == sprites.Count)
                    index = 0;
            }
        }

        private async UniTask ProgressText(string log, float duration) {
            int count = 1;
            while(duration > 0f) {
                this.textProgressLog.text = log;
                for(int i = 0; i < count; i++) {
                    this.textProgressLog.text += ".";
                }
                await UniTask.Delay(TimeSpan.FromSeconds(.1f));
                duration -= .1f;
                count++;
                if(count > 3) count = 1;
            }
        }
   
        private ItemSlot GetItemSlot() {
            ItemSlot slot = slotPool.Find(e => !e.gameObject.activeInHierarchy);

            if(slot == null) {
                slot = Instantiate(slotPrefab, slotContainer);
                slotPool.Add(slot);
            }

            slot.gameObject.SetActive(true);
            return slot;
        }
    }
}