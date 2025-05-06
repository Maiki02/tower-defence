using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [Header("Prefabs de Enemigos")]
    [SerializeField] private GameObject dwarfPrefab;
    [SerializeField] private GameObject normalPrefab;
    [SerializeField] private GameObject giantPrefab;

    [Header("Configuración de Pooling")]
    [SerializeField] private int initialPoolSize = 10;

    // Diccionario que almacena un pool para cada EnemyType
    private Dictionary<EnemyType, IObjectPool<GameObject>> pools;

    private void Awake()
    {
        // Singleton: aseguramos que solo haya una instancia de EnemySpawner
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Creamos los pools para cada tipo de enemigo al iniciar
        pools = new Dictionary<EnemyType, IObjectPool<GameObject>>
        {
            { EnemyType.Dwarf, CreatePool(dwarfPrefab) },
            { EnemyType.Normal, CreatePool(normalPrefab) },
            { EnemyType.Giant, CreatePool(giantPrefab) }
        };
    }

    // Crea un pool genérico para un prefab dado.
    private IObjectPool<GameObject> CreatePool(GameObject prefab)
    {
        return new ObjectPool<GameObject>(
            createFunc: () =>
            {
                // Instanciamos y desactivamos la instancia antes de devolverla
                var go = Instantiate(prefab);
                go.SetActive(false);
                return go;
            },
            actionOnGet: go => go.SetActive(true),       // Al obtener: activamos
            actionOnRelease: go => go.SetActive(false), // Al liberar: desactivamos
            actionOnDestroy: go => Destroy(go),         // Al destruir: eliminamos el GameObject
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: initialPoolSize * 4
        );
    }

    // Obtiene un enemigo del pool correspondiente y lo posiciona.
    public GameObject SpawnEnemy(EnemyType type, Vector3 position)
    {
        // Verificamos que el enemigo exista en el pool
        if (!pools.TryGetValue(type, out var pool)) return null;

        // Obtenemos la instancia, la activamos y la colocamos en posición
        var enemy = pool.Get();
        enemy.transform.position = position;
        return enemy;
    }

    // Devuelve un enemigo al pool correspondiente para reutilizarlo.
    public void DespawnEnemy(GameObject enemy, EnemyType type)
    {
        if (!pools.TryGetValue(type, out var pool))
        {
            Destroy(enemy);
            return;
        }

        // Liberamos la instancia para que quede disponible en el pool
        pool.Release(enemy);
    }
}
