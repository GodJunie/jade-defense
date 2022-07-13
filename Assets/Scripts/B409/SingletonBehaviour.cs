using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace B409 {
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {
        protected static T sInstance = null;

        public static T Instance {
            get {
                if(sInstance == null) {
                    sInstance = FindObjectOfType(typeof(T)) as T;

                    if(sInstance == null) {
                        Debug.Log("Nothing" + sInstance.ToString());
                        return null;
                    }
                }
                return sInstance;
            }
        }

        // Use this for initialization
        protected virtual void Awake() {
            DontDestroyOnLoad(gameObject);

            if(Instance != this)
                Destroy(gameObject);
        }
    }
}