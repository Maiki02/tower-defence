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
            int cantidad = oleada.count;

            // spawn de la oleada
            for (int j = 0; j < cantidad; j++)
            {
                Transform punto = spawnPoints[Random.Range(0, spawnPoints.Count)];
                EnemyType tipo = GetRandomEnemyType();
                enemySpawner.SpawnEnemy(tipo, punto.position);
                yield return new WaitForSeconds(oleada.spawnInterval);
            }

            // pausa entre oleadas
            if (i < waves.Count - 1)
                yield return new WaitForSeconds(timeBetweenWaves);

            // si ya fue la última oleada.
            if (i == waves.Count - 1)
            {
                // espera a que no quede ningún Enemy en escena
                yield return new WaitUntil(() => FindObjectsOfType<Enemy>().Length == 0);
                // llama al fin del juego
                GameController.Instance.FinishGame(GameOverType.PlayerWIN);
                yield break;
            }
        }
    }

    private EnemyType GetRandomEnemyType()
    {
        int totalTipos = System.Enum.GetValues(typeof(EnemyType)).Length;
        return (EnemyType)Random.Range(0, totalTipos);
    }
}
