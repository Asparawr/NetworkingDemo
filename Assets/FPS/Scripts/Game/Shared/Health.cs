using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public partial class Health : NetworkBehaviour
    {
        [Tooltip("Maximum amount of health")] public float MaxHealth = 10f;

        [Tooltip("Health ratio at which the critical health vignette starts appearing")]
        public float CriticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamaged;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        public float CurrentHealth { get; set; }
        public bool Invincible { get; set; }
        public bool CanPickup() => CurrentHealth < MaxHealth;

        public float GetRatio() => CurrentHealth / MaxHealth;
        public bool IsCritical() => GetRatio() <= CriticalHealthRatio;

        float deathInvurnabilityTime = .2f;
        float lastTimeDeath;

        void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void Heal(float healAmount)
        {
            if (!isServer) return; //ADDED check to only heal on server
            float healthBefore = CurrentHealth;
            CurrentHealth += healAmount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

            // call OnHeal action
            float trueHealAmount = CurrentHealth - healthBefore;
            if (trueHealAmount > 0f)
            {
                OnHealed?.Invoke(trueHealAmount);
            }
            RpcSetHealth(CurrentHealth);
        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            if (isServer){
                if (Invincible ||  Time.time - lastTimeDeath < deathInvurnabilityTime)
                    return;

                float healthBefore = CurrentHealth;
                CurrentHealth -= damage;
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

                // call OnDamage action
                RpcSetHealth(CurrentHealth);

                HandleDeath();
            }
            OnDamaged?.Invoke(1, damageSource);
        }

        public void Kill()
        {
            CurrentHealth = 0f;

            // call OnDamage action
            OnDamaged?.Invoke(MaxHealth, null);

            HandleDeath();
        }

        void HandleDeath()
        {
            // call OnDie action
            if (CurrentHealth <= 0f)
            {
                lastTimeDeath = Time.time;
                OnDie?.Invoke();
            }
        }
    }
}