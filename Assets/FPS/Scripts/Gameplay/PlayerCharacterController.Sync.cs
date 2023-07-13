using Mirror;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public partial class PlayerCharacterController : NetworkBehaviour
    {
    //ADDED target rpc to client player, to move player to spawnpoint
        [TargetRpc]
        public void RpcRespawn(NetworkConnectionToClient conn)
        {
            // Disable character controller and re-enable it in 0.1s to teleport
            if (isLocalPlayer){
                m_Controller.enabled = false;
                Invoke(nameof(EnableCharacterController), 0.1f);
                transform.position = Vector3.zero;
            }
        }

        void EnableCharacterController()
        {
            m_Controller.enabled = true;
        }
    }
}