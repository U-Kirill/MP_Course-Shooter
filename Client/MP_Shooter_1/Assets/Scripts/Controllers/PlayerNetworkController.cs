using System;
using Components;
using UnityEngine;

namespace Controllers
{
    public class PlayerNetworkController : MonoBehaviour
    {
        [SerializeField] private Server.Server _server;
        [SerializeField] private Move _move;

        private void Update()
        {
            if(!_move)
                return;
            
            _move.GetInfo(out Vector3 position);

            SendPositionData(position);
        }
        
        public void SetPlayerMove(Move move)
        {
            _move = move != null
                ? move
                : throw new ArgumentOutOfRangeException();
        }
        
        private void SendPositionData(Vector3 position)
        {
            _server.ReportPlayerPosition(new Vector3
            {
                x = position.x,
                y = position.z
            });
        }
    }
}