using Mirror;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    public partial class PlayerCharacterController : NetworkBehaviour
    {
        [TargetRpc]
        public void RpcRespawn(NetworkConnectionToClient conn)
        {
            // Disable character controller and re-enable it in 0.1s
            // to reset the character controller internal state.
            // This will prevent us from falling through the floor
            // after respawn.
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