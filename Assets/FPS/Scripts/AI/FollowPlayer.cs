using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.AI
{
    public class FollowPlayer : MonoBehaviour
    {
        Transform m_PlayerTransform;
        Vector3 m_OriginalOffset;

        bool initialized;
        void Start()
        {
            NewNetworkManager.OnPlayerInstantiated += OnPlayerInstantiated;
        }

        void OnPlayerInstantiated()
        {
            ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            if (actorsManager != null)
                m_PlayerTransform = actorsManager.Player.transform;
            else
            {
                enabled = false;
                return;
            }

            m_OriginalOffset = transform.position - m_PlayerTransform.position;
            initialized = true;
        }

        void LateUpdate()
        {
            if (!initialized)
                return;
            transform.position = m_PlayerTransform.position + m_OriginalOffset;
        }
    }
}