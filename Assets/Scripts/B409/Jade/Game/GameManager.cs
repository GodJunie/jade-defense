using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409.Jade.Game {
    public class GameManager : SingletonBehaviour<GameManager> {
        #region Members 
        public GameProgress Progress { get; private set; }
        #endregion

        #region Mono
        protected override void Awake() {
            base.Awake();
            Progress = new GameProgress();
        }
        #endregion

        #region Farming

        #endregion
    }
}