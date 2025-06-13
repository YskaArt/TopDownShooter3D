using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float checkRadius = 1.5f;
    [SerializeField] private float spawnAreaSize = 20f;
    [SerializeField] private List<GameObject> enemiesPrefabs; 

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public bool TrySpawnEnemy(out GameObject enemyInstance)
    {
        if (enemiesPrefabs == null || enemiesPrefabs.Count == 0)
        {
            
            enemyInstance = null;
            return false;
        }

        for (int i = 0; i < 20; i++) 
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spawnAreaSize, spawnAreaSize),
                0,
                Random.Range(-spawnAreaSize, spawnAreaSize)
            );

            if (!IsVisibleToCamera(randomPos) && !Physics.CheckSphere(randomPos, checkRadius, obstructionMask))
            {
                // Elegir un prefab al azar de la lista
                int idx = Random.Range(0, enemiesPrefabs.Count);

                GameObject chosen = enemiesPrefabs[idx];

                enemyInstance = Instantiate(chosen, randomPos, Quaternion.identity);

                return true;
            }
        }

        enemyInstance = null;
        return false;
    }


    public void SpawnRandomEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            TrySpawnEnemy(out _);
        }
    }


    public bool IsVisibleToCamera(Vector3 worldPos)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPos);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnAreaSize);
    }
}
