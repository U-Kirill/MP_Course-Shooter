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
        private List<string> _wasChanged;
        
        private void OnEnable()
        {
            _ememiesStorage.OnActorAdded += OnActorAdded;
            _ememiesStorage.OnActorRemoved += OnActorRemoved;
            _server.OnStateRefreshed += OnStateRefreshed;
        }

        private void OnDisable()
        {
            _ememiesStorage.OnActorAdded -= OnActorAdded;
            _ememiesStorage.OnActorRemoved -= OnActorRemoved;
            _server.OnStateRefreshed -= OnStateRefreshed;
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

        private void OnStateRefreshed(State state)
        {
            state.players.ForEach((sessionId, player) =>
            {
                if(!_enemies.TryGetValue(sessionId, out Move enemyMove))
                    return;
                
                if(_wasChanged.Contains(sessionId))
                    return;
                
                //этот метод не тормозит, потому что не вызывается
                enemyMove.SetMovement(Vector3.zero);
            });
            
            _wasChanged.Clear();
        }

        private void OnStateRefreshed(string sessionId, List<DataChange> changes)
        {
            if(!_enemies.TryGetValue(sessionId, out Move enemyMove))
                return;
            
            enemyMove.GetInfo(out Vector3 position);
            
            Vector3 currentPos = position;
            Vector3 oldPos = position;

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
            enemyMove.SetPosition(currentPos);
            enemyMove.SetMovement(direction);
            _wasChanged.Add(sessionId);
        }
    }
}