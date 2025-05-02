using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private IObjectPool<GameObject> pool;

    void Awake()
    {
        this.CreatePool();
    }

    private void CreatePool() { 
        pool = new ObjectPool<GameObject>(
            createFunc:    () => { var go = Instantiate(enemyPrefab); go.SetActive(false); return go; },
            actionOnGet:   go => go.SetActive(true),
            actionOnRelease: go => go.SetActive(false),
            actionOnDestroy: go => Destroy(go),
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: initialPoolSize * 4
        );
    }

    public GameObject SpawnEnemy(Vector3 position)
    {
        var enemy = pool.Get();
        enemy.transform.position = position;
        return enemy;
    }

    public void DespawnEnemy(GameObject enemy)
    {
        pool.Release(enemy);
    }
}
