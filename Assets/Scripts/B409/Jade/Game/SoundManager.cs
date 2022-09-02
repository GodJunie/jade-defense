using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace B409.Jade.Game {
    public class SoundManager : SingletonBehaviour<SoundManager> {
        [SerializeField]
        private AudioSource bgmSource;
        [SerializeField]
        private int sfxPoolCount;

        private List<AudioSource> sfxSources = new List<AudioSource>();

        public float BgmVolume { get; private set; }
        public float SfxVolume { get; private set; }
        public bool BgmMute { get; private set; }
        public bool SfxMute { get; private set; }

        private const string BgmVolumeKey = "Bgm_Volume";
        private const string SfxVolumeKey = "Sfx_Volume";
        private const string BgmMuteKey = "Bgm_Mute";
        private const string SfxMuteKey = "Sfx_Mute";


        protected override void Awake() {
            base.Awake();

            for(int i = 0; i < sfxPoolCount; i++) {
                var g = new GameObject();
                g.transform.SetParent(this.transform);
                var source = g.AddComponent<AudioSource>();
                this.sfxSources.Add(source);
            }

            var bgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, 1f);
            var sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f);

            var bgmMute = PlayerPrefs.GetInt(BgmMuteKey, 0) == 1;
            var sfxMute = PlayerPrefs.GetInt(SfxMuteKey, 0) == 1;

            SetBgmVolume(bgmVolume);
            SetSfxVolume(sfxVolume);

            MuteBgm(bgmMute);
            MuteSfx(sfxMute);
        }

        public void PlaySfx(AudioClip clip) {
            var source = this.sfxSources.Find(e => !e.isPlaying);
            if(source == null)
                return;

            source.clip = clip;
            source.Play();
        }

        public void PlayBgm(AudioClip clip, float duration = 1f) {
            if(bgmSource.clip == clip)
                return;

            bgmSource.volume = 0f;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();

            bgmSource.DOFade(BgmVolume, duration);
        }

        public async void BgmOff(float duration = 1f) {
            await bgmSource.DOFade(0f, duration);
            bgmSource.Stop();
            bgmSource.clip = null;
        }

        public void SetBgmVolume(float volume) {
            this.bgmSource.volume = volume;

            this.BgmVolume = volume;
            PlayerPrefs.SetFloat(BgmVolumeKey, BgmVolume);
        }

        public void SetSfxVolume(float volume) {
            foreach(var source in this.sfxSources) {
                source.volume = volume;
            }

            this.SfxVolume = volume;
            PlayerPrefs.SetFloat(SfxVolumeKey, SfxVolume);
        }

        public void MuteBgm(bool mute) {
            this.bgmSource.mute = mute;

            this.BgmMute = mute;
            PlayerPrefs.SetInt(BgmMuteKey, mute ? 1 : 0);
        }

        public void MuteSfx(bool mute) {
            foreach(var source in this.sfxSources) {
                source.mute = mute;
            }

            this.SfxMute = mute;
            PlayerPrefs.SetInt(SfxMuteKey, mute ? 1 : 0);
        }
    }
}