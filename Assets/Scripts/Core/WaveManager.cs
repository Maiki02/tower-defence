using System.Collections;
using UnityEngine;
/*
public class Wave
{
        public int count;
        public float interval;
}*/

public class WaveGenerator : MonoBehaviour
{
    public Transform[] spawnPoints;
    private EnemySpawner spawner;

    void Awake()
    {
        spawner = FindObjectOfType<EnemySpawner>();
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // cada 4 s escoge un spawnPoint al azar
            int i = Random.Range(0, spawnPoints.Length);
            Vector3 pos = spawnPoints[i].position;

            spawner.SpawnEnemy(pos);

            yield return new WaitForSeconds(4f);
        }
    }
}

