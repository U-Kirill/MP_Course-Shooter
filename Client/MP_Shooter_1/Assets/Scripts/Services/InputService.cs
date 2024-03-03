using System;
using UnityEngine;

namespace Services
{
    public class InputService : MonoBehaviour
    {
        public event Action<Vector2> OnInput;

        private void Update()
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                movement += Vector3.up;
            if(Input.GetKey(KeyCode.A))
                movement += Vector3.left;
            if (Input.GetKey(KeyCode.S))
                movement += Vector3.down;
            if(Input.GetKey(KeyCode.D))
                movement += Vector3.right;
            
            OnInput?.Invoke(movement);
        }
    }
}