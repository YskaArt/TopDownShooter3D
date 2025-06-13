using UnityEngine;

public class RandomPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private Transform areaCenter;
    [SerializeField] private Vector3 areaSize = new Vector3(20, 0, 20);
    [SerializeField] private float spawnInterval = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPickup), spawnInterval, spawnInterval);
    }


    private void SpawnPickup()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(areaCenter.position.x - areaSize.x / 2, areaCenter.position.x + areaSize.x / 2),
            areaCenter.position.y,
            Random.Range(areaCenter.position.z - areaSize.z / 2, areaCenter.position.z + areaSize.z / 2)
        );

        Instantiate(pickupPrefab, randomPos, Quaternion.identity);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(areaCenter.position, areaSize);
    }
}
