using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Server
{
    public class Server : MonoBehaviour
    {
        private const string MoveMessageKey = "move";
        
        [SerializeField] private ServerConnection _serverConnection;

        public event Action<State> OnInitialized;
        public event Action<string, Player> OnPlayerAdded;
        public event Action<string, Player> OnPlayerRemoved;
        public event Action<State> OnStateRefreshed;

        public string SessionId => _serverConnection.SessionId;

        private void OnEnable()
        {
            _serverConnection.OnInitialized += OnInitializedHandler;
            _serverConnection.OnStateRefreshed += OnRefreshStateHandler;
        }

        private void OnDisable()
        {
            _serverConnection.OnInitialized -= OnInitializedHandler;
            _serverConnection.OnStateRefreshed -= OnRefreshStateHandler;
        }

        public void ReportPlayerPosition(Vector2 position)
        {
            Send(MoveMessageKey, new Dictionary<string, object>
            {
                ["x"] = position.x,
                ["y"] = position.y
            });
        }

        private void OnInitializedHandler(State state)
        {
            state.players.OnAdd += PlayerAddHandler;
            state.players.OnRemove += PlayerRemoveHandler;
            
            OnInitialized?.Invoke(state);
        }

        private void OnRefreshStateHandler(State state)
        {
            OnStateRefreshed?.Invoke(state);
        }

        private void PlayerAddHandler(string key, Player value) => OnPlayerAdded?.Invoke(key, value);

        private void PlayerRemoveHandler(string key, Player value) => OnPlayerRemoved?.Invoke(key, value);
        
        private void Send(string key, Dictionary<string, object> data) => _serverConnection.Send(key, data);
    }
}