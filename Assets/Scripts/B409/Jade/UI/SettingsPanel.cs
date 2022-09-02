using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace B409.Jade.UI {
    using Game;

    public class SettingsPanel : MonoBehaviour {
        [SerializeField]
        private Slider bgmSlider;
        [SerializeField]
        private Slider sfxSlider;

        [SerializeField]
        private Button buttonBgmMute;
        [SerializeField]
        private Button buttonSfxMute;
        [SerializeField]
        private Sprite spriteMuteOn;
        [SerializeField]
        private Sprite spriteMuteOff;

        public void Open() {
            this.gameObject.SetActive(true);

            bgmSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.RemoveAllListeners();

            bgmSlider.value = SoundManager.Instance.BgmVolume;
            sfxSlider.value = SoundManager.Instance.SfxVolume;

            bgmSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetBgmVolume(value));
            sfxSlider.onValueChanged.AddListener(value => SoundManager.Instance.SetSfxVolume(value));

            this.buttonBgmMute.image.sprite = SoundManager.Instance.BgmMute ? spriteMuteOn : spriteMuteOff;
            this.buttonSfxMute.image.sprite = SoundManager.Instance.SfxMute ? spriteMuteOn : spriteMuteOff;

            Time.timeScale = 0f;
        }

        public void MuteBgm() {
            SoundManager.Instance.MuteBgm(!SoundManager.Instance.BgmMute);

            this.buttonBgmMute.image.sprite = SoundManager.Instance.BgmMute ? spriteMuteOn : spriteMuteOff;
        }

        public void MuteSfx() {
            SoundManager.Instance.MuteSfx(!SoundManager.Instance.SfxMute);

            this.buttonSfxMute.image.sprite = SoundManager.Instance.SfxMute ? spriteMuteOn : spriteMuteOff;
        }

        public void Close() {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }
}