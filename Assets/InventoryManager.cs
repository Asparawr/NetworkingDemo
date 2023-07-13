using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.FPS;
using UnityEngine;

public static class InventoryManager
{
    private static List<Action<List<string>>> pendingInventoryRequests = new List<Action<List<string>>>();
    private static List<Action<bool>> pendingInventoryAddRequests = new List<Action<bool>>();
    public static Dictionary<string, List<string>> inventories = new Dictionary<string, List<string>>();


    [RuntimeInitializeOnLoadMethod]
    private static void OnInitiated() {
        NewNetworkManager.OnServerStart -= OnServerStart;
        NewNetworkManager.OnServerStart += OnServerStart;
        NewNetworkManager.OnServerStop -= OnServerStop;
        NewNetworkManager.OnServerStop += OnServerStop;
        NewNetworkManager.OnClientStart -= OnClientStart;
        NewNetworkManager.OnClientStart += OnClientStart;
        NewNetworkManager.OnClientStop -= OnClientStop;
        NewNetworkManager.OnClientStop += OnClientStop;
    }

    private static void OnServerStart() {
        NetworkServer.RegisterHandler<RequestInventory>(OnRequestInventory);
        NetworkServer.RegisterHandler<RequestAddItem>(OnRequestAddItem);
    }

    private static void OnServerStop() {
        NetworkServer.UnregisterHandler<ResponseInventory>();
        NetworkServer.UnregisterHandler<RequestAddItem>();
    }
    private static void OnClientStart() {
        NetworkClient.RegisterHandler<ResponseInventory>(OnResponseInventory);
        NetworkClient.RegisterHandler<ResponseAddItem>(OnResponseAddItem);
    }

    private static void OnClientStop() {
        NetworkClient.UnregisterHandler<ResponseInventory>();
        NetworkClient.UnregisterHandler<ResponseAddItem>();
    }

    struct RequestInventory : NetworkMessage {
    }

    struct ResponseInventory : NetworkMessage {
        public string playerIndentity;
        public List<string> inventory;
    }

    struct RequestAddItem : NetworkMessage {
        public string itemName;
    }
    struct ResponseAddItem : NetworkMessage {
        public bool success;
    }

    public static void Clear() {
        inventories.Clear();
    }

    public static void GetInventory(Action<List<string>> callback) {
        pendingInventoryRequests.Add(callback);
        NetworkClient.Send(new RequestInventory( ));
    }

    private static void OnRequestInventory(NetworkConnection conn, RequestInventory msg) {
        string playerId = conn.identity.netId.ToString();

        ResponseInventory responseInventory = new ResponseInventory();

        if (!inventories.ContainsKey(playerId)) {
            inventories.Add(playerId, new List<string>());
        }
        
        responseInventory.inventory = inventories[playerId];
        
        conn.Send(responseInventory);
    }

    private static void OnResponseInventory(ResponseInventory msg) {
        foreach (Action<List<string>> callback in pendingInventoryRequests) {
            callback(msg.inventory);
        }
        
        pendingInventoryRequests.Clear();
    }

    public static void AddItem(string item, Action<bool> callback) {
        pendingInventoryAddRequests.Add(callback);
        NetworkClient.Send(new RequestAddItem() {
            itemName = item
        });
    }

    public static void RemoveItem(string item, Action<bool> callback) {
        AddItem(item, callback);
    }

    private static void OnRequestAddItem(NetworkConnection conn, RequestAddItem msg) {
        string playerId = conn.identity.netId.ToString();
        string itemName = msg.itemName;

        ResponseAddItem ResponseAddItem = new ResponseAddItem();

        if (!inventories.ContainsKey(playerId)) {
            inventories.Add(playerId, new List<string>());
        }
        // count items containing the same name
        int count = 0;
        foreach (var item in inventories[playerId]) {
            if (item.Contains(itemName)) {
                count++;
            }
        }
        // add item with count
        itemName += count;
        inventories[playerId].Add(itemName);
        ResponseAddItem.success = true;
        
        conn.Send(ResponseAddItem);
    }

    private static void OnResponseAddItem(ResponseAddItem msg) {
        foreach (Action<bool> callback in pendingInventoryAddRequests) {
            callback(msg.success);
        }
        
        pendingInventoryAddRequests.Clear();
    }
}