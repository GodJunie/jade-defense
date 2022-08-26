using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace B409.Jade.Game {
    public class SoundManager : SingletonBehaviour<SoundManager> {
        [SerializeField]
        private AudioSource bgmSource;
        [SerializeField]
        private int sfxPoolCount;

        private List<AudioSource> sfxSources = new List<AudioSource>();


        protected override void Awake() {
            base.Awake();

            for(int i = 0; i < sfxPoolCount; i++) {
                var g = new GameObject();
                g.transform.SetParent(this.transform);
                var source = g.AddComponent<AudioSource>();
                this.sfxSources.Add(source);
            }
        }

        public void PlaySfx(AudioClip clip) {
            var source = this.sfxSources.Find(e => !e.isPlaying);
            if(source == null)
                return;

            source.clip = clip;
            source.Play();
        }

        public void PlayBgm(AudioClip clip) {
            if(bgmSource.clip == clip)
                return;
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
}