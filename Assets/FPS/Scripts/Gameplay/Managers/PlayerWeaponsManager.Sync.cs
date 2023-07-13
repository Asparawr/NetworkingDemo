using Mirror;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Gameplay
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public partial class PlayerWeaponsManager : NetworkBehaviour
    { //ADDED weapon switch
        
        [Command]
        public void CmdSwitchToWeaponIndex(int index, bool force)
        {
            // set the weapon
            SwitchToWeaponIndex(index, force);
            // set it on the server
            RpcSwitchToWeaponIndex(index, force);
        }

        [ClientRpc]
        void RpcSwitchToWeaponIndex(int index, bool force)
        {
            // set the weapon
            SwitchToWeaponIndex(index, force);
        }
        
    }
}