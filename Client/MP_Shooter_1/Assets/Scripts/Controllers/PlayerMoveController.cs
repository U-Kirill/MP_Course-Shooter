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

        private void OnInput(Vector2 input)
        {
            _playerMove.SetMovement(new Vector3(input.x, 0, input.y));
        }
    }
}