using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

namespace B409.Jade.UI {
    public class DialogueEvent : MonoBehaviour {
        [SerializeField]
        [ListDrawerSettings(ListElementLabelName = "Id")]
        private List<EventInfo> eventList;

        private Dictionary<string, UniTaskCompletionSource> sources = new Dictionary<string, UniTaskCompletionSource>();

        public async UniTask EventStart(string id) {
            var info = this.eventList.Find(e => e.Id == id);

            if(info == null) {
                throw new Exception(string.Format("Event is not exists, name: {0}", id));
            }

            info.OnEventStart.Invoke();
            sources[id] = new UniTaskCompletionSource();
            await sources[id].Task;
        }

        public void EventEnd(string id) {
            if(!sources.ContainsKey(id)) {
                throw new Exception(string.Format("There is no completion source, id: {0}", id));
            }
            sources[id].TrySetResult();
        }

        [Serializable]
        public class EventInfo {
            [BoxGroup("Id")]
            [HideLabel]
            [SerializeField]
            private string id;
            [BoxGroup("Event")]
            [SerializeField]
            private UnityEvent onEventStart;

            public string Id => id;
            public UnityEvent OnEventStart => onEventStart;
        }
    }
}