using System;
using Mirror;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public partial class ItemPickup : Pickup
    {
        //ADDED pickup item, handle sending requests to add item to inventory and message to destroy it

        public string ItemName = "ItemCube";
        bool picked = false;
        public struct DestroyMessage : NetworkMessage
        {
            public GameObject item;
        }
        public override void Start()
        {
            NetworkServer.ReplaceHandler<DestroyMessage>(OnDestroyMessage);
            base.Start();
        }
        void OnDestroyMessage(NetworkConnectionToClient client, DestroyMessage msg)
        {
            Destroy(msg.item);
        }
        protected override void OnPicked(PlayerCharacterController player)
        {
            if (picked || !player.isLocalPlayer) return;
            picked = true;
            InventoryManager.AddItem(ItemName, (success) => { 
                if (success) {
                    InventoryManager.GetInventory((inventory) => { 
                        foreach (var item in inventory) {
                            Debug.Log(item);
                        }
                        DestroyMessage networkMessage = new DestroyMessage{ item = gameObject };
                        NetworkClient.Send(networkMessage);
                    });
                }
            });
        }
    }
}