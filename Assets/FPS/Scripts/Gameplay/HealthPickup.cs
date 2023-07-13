using System;
using Mirror;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class HealthPickup : Pickup
    {
        public struct HealMessage : NetworkMessage
        {
            public float healAmount;
            public PlayerCharacterController player;
        }
        public override void Start()
        {
            NetworkServer.ReplaceHandler<HealMessage>(OnHeal);
            base.Start();
        }

        private void OnHeal(NetworkConnectionToClient client, HealMessage msg)
        {
            msg.player.Health.Heal(msg.healAmount);
        }

        [Header("Parameters")] [Tooltip("Amount of health to heal on pickup")]
        public float HealAmount;

        protected override void OnPicked(PlayerCharacterController player)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth && playerHealth.CanPickup() && player.isLocalPlayer)
            {
                // Send heal message to server 
                HealMessage networkMessage = new HealMessage {healAmount = HealAmount, player = player};
                NetworkClient.Send(networkMessage);
                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }
}