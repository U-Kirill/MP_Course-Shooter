using System;
using Services;
using UnityEngine;

namespace Controllers
{
    public class ActorsCreator : MonoBehaviour
    {
        [SerializeField] private Server.Server _server;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private ActorStorage _playersStorage;
        [SerializeField] private ActorStorage _enemyStorage;

        private void OnEnable()
        {
            _server.OnInitialized += OnInitialized;
            _server.OnPlayerAdded += OnPlayerAdded;
            _server.OnPlayerRemoved += OnPlayerRemoved;
        }

        private void OnDisable()
        {
            _server.OnInitialized -= OnInitialized;
            _server.OnPlayerAdded -= OnPlayerAdded;
            _server.OnPlayerRemoved -= OnPlayerRemoved;
        }

        private void OnInitialized(State state)
        {
            state.players.ForEach(CreatePlayerOrEnemy);
        }

        private void OnPlayerAdded(string sessionId, Player player)
        {
            CreatePlayerOrEnemy(sessionId, player);
        }

        private void OnPlayerRemoved(string sessionId, Player player)
        {
            if(IsPlayer(sessionId))
                _playersStorage.RemoveActor(sessionId);
            else
                _enemyStorage.RemoveActor(sessionId);
        }

        private void CreatePlayer(string sessionId, Player player)
        {
            GameObject playerInstance = Instantiate(_playerPrefab, new Vector3(player.x, 0, player.y), Quaternion.identity);
            _playersStorage.AddActor(sessionId, playerInstance, player);
        }

        private void CreateEnemy(string sessionId, Player player)
        {
            GameObject enemyInstance = Instantiate(_enemyPrefab, new Vector3(player.x, 0, player.y), Quaternion.identity);
            _enemyStorage.AddActor(sessionId, enemyInstance, player);
        }

        private void CreatePlayerOrEnemy(string sessionId, Player player)
        {
            if (IsPlayer(sessionId))
                CreatePlayer(sessionId, player);
            else
                CreateEnemy(sessionId, player);
        }

        private bool IsPlayer(string sessionId) => sessionId == _server.SessionId;
    }
}