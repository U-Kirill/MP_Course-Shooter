using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class ActorStorage : MonoBehaviour
    {
        private List<Data> _actors = new List<Data>();
        
        public event Action<Data> OnActorAdded;
        public event Action<Data> OnActorRemoved;

        public IReadOnlyCollection<Data> Actors => _actors;
        
        public void AddActor(string sessionId, GameObject actor, Player player)
        {
            var data = new Data
            {
                SessionId = sessionId,
                View = actor,
                Player = player
            };
            
            _actors.Add(data);
            OnActorAdded?.Invoke(data);
        }

        public void RemoveActor(string sessionId)
        {
            Data data = _actors.Find(x => x.SessionId == sessionId);
            
            _actors.Remove(data);
            OnActorRemoved?.Invoke(data);
        }

        public class Data
        {
            public string SessionId;
            public GameObject View;
            public Player Player;
        }
    }
}