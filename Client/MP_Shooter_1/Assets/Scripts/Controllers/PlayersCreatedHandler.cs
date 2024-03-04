using System;
using Components;
using Services;
using UnityEngine;

namespace Controllers
{
    public class PlayersCreatedHandler : MonoBehaviour
    {
        [SerializeField] private ActorStorage _playerStorage;
        [SerializeField] private PlayerMoveController _playerMoveController;
        [SerializeField] private PlayerNetworkController _playerNetworkController;

        private void OnEnable()
        {
            _playerStorage.OnActorAdded += OnPlayerCreated;
        }

        private void OnDisable()
        {
            _playerStorage.OnActorAdded -= OnPlayerCreated;
        }

        private void OnPlayerCreated(ActorStorage.Data data)
        {
            var playerMove = data.View.GetComponent<Move>();
            
            _playerMoveController.SetPlayerMove(playerMove);
            _playerNetworkController.SetPlayerMove(playerMove);
        }
    }
}