
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.FPS.Game
{
    public abstract class ProjectileBase : NetworkBehaviour
    {
        [SyncVar]
        public GameObject Owner;
        [SyncVar]
        public Vector3 InitialPosition;
        [SyncVar]
        public Vector3 InitialDirection;
        [SyncVar]
        public Vector3 InheritedMuzzleVelocity;
        [SyncVar]
        public float InitialCharge;

        public UnityAction OnShoot;

        public void Shoot(WeaponController controller)
        {
            Owner = controller.Owner;
            InitialPosition = transform.position;
            InitialDirection = transform.forward;
            InheritedMuzzleVelocity = controller.MuzzleWorldVelocity;
            InitialCharge = controller.CurrentCharge;

            OnShoot?.Invoke();
        }
        public void Start()
        {
            OnShoot?.Invoke();
        }
    }
}