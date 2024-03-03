using System;
using UnityEngine;

namespace Components
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;
        
        private Vector3 _movement;

        private void Update()
        {
            transform.position += _movement * Time.deltaTime;
        }

        public void SetPosition(Vector3 to)
        {
            transform.position = to;
        }

        public void SetMovement(Vector3 direction)
        {
            _movement = direction.normalized * _speed;
        }

        public void GetInfo(out Vector3 position)
        {
            position = transform.position;
        }
    }
}
