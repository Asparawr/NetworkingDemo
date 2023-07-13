using System.Collections.Generic;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Discovery
{
    [DisallowMultipleComponent]
    public class ServerListController : MonoBehaviour
    {
        // Based on NetworkDiscoveryHUD
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        Vector2 scrollViewPos = Vector2.zero;

        public GameObject serverListGrid;
        public GameObject serverEntryPrefab;
        public Button HostButton;
        public Button RefreshButton;
        List<GameObject> serverEntries = new List<GameObject>();

        public NetworkDiscovery networkDiscovery;

#if UNITY_EDITOR
        void OnValidate()
        {
            if (networkDiscovery == null)
            {
                networkDiscovery = GetComponent<NetworkDiscovery>();
                UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
                UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
            }
        }
#endif

        void Start()
        {
            networkDiscovery.StartDiscovery();
            
            HostButton.onClick.AddListener(() =>
            {
                discoveredServers.Clear();
                networkDiscovery.AdvertiseServer();
                NetworkManager.singleton.StartHost();
            });

        }

        void Connect(ServerResponse info)
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(info.uri);
        }

        public virtual void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
            // Update server entries
            foreach (GameObject entry in serverEntries)
            {
                Destroy(entry);
            }
            serverEntries.Clear();
            foreach (ServerResponse server in discoveredServers.Values)
            {
                GameObject entry = Instantiate(serverEntryPrefab, serverListGrid.transform);
                entry.GetComponent<ServerEntry>()?.Populate(server);
                serverEntries.Add(entry);
                entry.GetComponent<Button>().onClick.AddListener(() =>
                {
                    networkDiscovery.StopDiscovery();
                    NetworkManager.singleton.StartClient(server.uri);
                });
            }
        }
    }
}
