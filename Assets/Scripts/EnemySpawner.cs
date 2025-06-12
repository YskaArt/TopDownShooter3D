using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private LayerMask obstructionMask;
    [SerializeField] private float checkRadius = 1.5f;
    [SerializeField] private float spawnAreaSize = 20f;
    [SerializeField] private GameObject enemyPrefab;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public bool TrySpawnEnemy(out GameObject enemyInstance)
    {
        for (int i = 0; i < 20; i++) // Máximo 20 intentos
        {
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spawnAreaSize, spawnAreaSize),
                0,
                Random.Range(-spawnAreaSize, spawnAreaSize)
            );

            if (!IsVisibleToCamera(randomPos) && !Physics.CheckSphere(randomPos, checkRadius, obstructionMask))
            {
                enemyInstance = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
                return true;
            }
        }

        enemyInstance = null;
        return false;
    }

    private bool IsVisibleToCamera(Vector3 worldPos)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(worldPos);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnAreaSize);
    }
#endif
}
