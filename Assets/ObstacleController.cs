using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.FPS.Game;
using UnityEngine;

public class ObstacleController : NetworkBehaviour
{
    public List<GameObject> LootPrefabs;

    bool IsDead = false;
    
    public void Start()
    {
        GetComponent<Health>().OnDie += OnDie;
    }

    void OnDie()
    {
        if (IsDead) return;
        foreach (var lootPrefab in LootPrefabs)
        {
            var loot = Instantiate(lootPrefab, transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.identity);
            NetworkServer.Spawn(loot);
        }
        IsDead = true;
        // this will call the OnDestroy function
        Destroy(gameObject);
    }
}
