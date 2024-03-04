using System;
using System.Collections.Generic;
using Colyseus.Schema;
using Components;
using Services;
using UnityEngine;

namespace Controllers
{
    public class EnemiesNetworkController : MonoBehaviour
    {
        [SerializeField] private Server.Server _server;
        [SerializeField] private ActorStorage _ememiesStorage;

        private Dictionary<string, Move> _enemies = new Dictionary<string, Move>(); 
        
        private void OnEnable()
        {
            _ememiesStorage.OnActorAdded += OnActorAdded;
            _ememiesStorage.OnActorRemoved += OnActorRemoved;
        }

        private void OnDisable()
        {
            _ememiesStorage.OnActorAdded -= OnActorAdded;
            _ememiesStorage.OnActorRemoved -= OnActorRemoved;
        }

        private void OnActorAdded(ActorStorage.Data data)
        {
            _enemies.Add(data.SessionId, data.View.GetComponent<Move>());
            
            data.Player.OnChange += changes =>
            {
                OnStateRefreshed(data.SessionId, changes);
            };
        }

        private void OnActorRemoved(ActorStorage.Data data)
        {
            _enemies.Remove(data.SessionId);
        }

        private void OnStateRefreshed(string sessionId, List<DataChange> changes)
        {
            Vector3 currentPos = Vector3.zero;
            Vector3 oldPos = Vector3.zero;

            foreach (DataChange change in changes)
            {
                switch (change.Field)
                {
                    case "x":
                        currentPos.x = (float)change.Value;
                        oldPos.x = (float)change.PreviousValue;
                        break;
                    case "y":
                        currentPos.z = (float)change.Value;
                        oldPos.z = (float)change.PreviousValue;
                        break;
                }
            }

            Vector3 direction = currentPos - oldPos;

            if(!_enemies.TryGetValue(sessionId, out Move enemyMove))
                return;
            
            enemyMove.SetPosition(currentPos);
            enemyMove.SetMovement(direction);
        }
    }
}