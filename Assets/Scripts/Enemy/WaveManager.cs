using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Referencias necesarias")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Configuración de Oleadas")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private float timeBetweenWaves = 3f;

    private void Start()
    {
        // Validaciones mínimas
        if (enemySpawner == null) Debug.LogError("WaveManager: falta asignar EnemySpawner.");
        if (spawnPoints == null || spawnPoints.Count == 0) Debug.LogError("WaveManager: falta asignar SpawnPoints.");
        if (waves == null || waves.Count == 0) Debug.LogError("WaveManager: falta configurar Waves.");

        // Arrancamos la gestión de oleadas
        StartCoroutine(SpawnAllWaves());
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int i = 0; i < waves.Count; i++)
        {
            Wave oleada = waves[i];
            // Aumentamos la cantidad de enemigos conforme sube el índice de oleada
            int cantidad = oleada.count + i;

            for (int j = 0; j < cantidad; j++)
            {
                // Elegimos un punto de spawn al azar
                Transform punto = spawnPoints[Random.Range(0, spawnPoints.Count)];
                // Elegimos un tipo de enemigo al azar
                EnemyType tipo = GetRandomEnemyType();
                // Le pedimos al spawner que lo cree en ese punto
                enemySpawner.SpawnEnemy(tipo, punto.position);
                yield return new WaitForSeconds(oleada.spawnInterval);
            }

            // Pausa entre oleadas (menos al final)
            if (i < waves.Count - 1)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    private EnemyType GetRandomEnemyType()
    {
        int totalTipos = System.Enum.GetValues(typeof(EnemyType)).Length;
        return (EnemyType)Random.Range(0, totalTipos);
    }
}
