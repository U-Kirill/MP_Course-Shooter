using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ActorStorage : MonoBehaviour
    {
        private List<Data> _actors = new List<Data>();
        
        public event Action<GameObject> OnActorAdded;
        public event Action<GameObject> OnActorRemoved;

        public void AddActor(string sessionId, GameObject actor)
        {
            _actors.Add(new Data
            {
                SessionId = sessionId,
                Actor = actor
            });
            OnActorAdded?.Invoke(actor);
        }

        public void RemoveActor(GameObject actor)
        {
            _actors.RemoveAll(x => x.Actor == actor);
            OnActorRemoved?.Invoke(actor);
        }

        public void RemoveActor(string sessionId)
        {
            Data data = _actors.Find(x => x.SessionId == sessionId);
            _actors.Remove(data);
            OnActorRemoved?.Invoke(data.Actor);
        }
        
        private class Data
        {
            public string SessionId;
            public GameObject Actor;
        }
    }
}