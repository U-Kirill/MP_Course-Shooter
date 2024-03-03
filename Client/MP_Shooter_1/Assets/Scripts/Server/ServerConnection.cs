using System;
using System.Collections.Generic;
using Colyseus;

namespace Server
{
    public class ServerConnection : ColyseusManager<ServerConnection>
    {
        private const string StateHandler = "state_handler";

        private ColyseusRoom<State> _room;

        public event Action<State> OnInitialized;
        public event Action<string, Player> OnPlayerAdded;
        public event Action<string, Player> OnPlayerRemoved;
        public event Action<State> OnStateRefreshed;
        
        public string SessionId => _room.SessionId;
        
        protected override void Awake()
        {
            base.Awake();
            
            Instance.InitializeClient();
            Connect();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _room.Leave();
        }

        public void Send(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }
        
        private async void Connect()
        {
            _room = await Instance.client.JoinOrCreate<State>(StateHandler);
            _room.OnStateChange += OnStateChanged;
        }

        private void OnStateChanged(State state, bool isFirstState)
        {
            if (isFirstState)
                Initialize(state);
            else
                RefreshState(state);
        }

        private void Initialize(State state) => OnInitialized?.Invoke(state);
        

        private void RefreshState(State state)
        {
            OnStateRefreshed?.Invoke(state);
        }
    }
}