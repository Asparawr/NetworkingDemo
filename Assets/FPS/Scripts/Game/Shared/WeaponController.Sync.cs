﻿using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public partial class WeaponController : NetworkBehaviour
    { //ADDED player command for shooting
        [Command]
        void CmdHandleShoot(float currentCharge)
        {
            RpcHandleShoot(currentCharge);
            HandleShoot(currentCharge);
        }

        [ClientRpc]
        void RpcHandleShoot(float currentCharge)
        {
            HandleShoot(currentCharge);
        }
    }
}