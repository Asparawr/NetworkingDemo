﻿using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public partial class Health : NetworkBehaviour
    {
        [ClientRpc]
        public void RpcSetHealth(float amount)
        { //ADDED set health for each client
            CurrentHealth = amount;
        }
    }
}