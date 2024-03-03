using System;
using Components;
using Services;
using UnityEngine;

namespace Controllers
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private InputService _inputService;
        [SerializeField] private Move _playerMove;

        public void OnEnable() => _inputService.OnInput += OnInput;
        public void OnDisable() => _inputService.OnInput -= OnInput;

        public void SetPlayerMove(Move move)
        {
            _playerMove = move != null
                ? move
                : throw new ArgumentOutOfRangeException();
        }

        private void OnInput(Vector2 input)
        {
            if(!_playerMove)
                return;
            
            _playerMove.SetMovement(new Vector3(input.x, 0, input.y));
        }
    }
}