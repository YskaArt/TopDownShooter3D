using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private int totalWaves = 3;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float delayBetweenWaves = 5f;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    public delegate void VictoryEvent();
    public event VictoryEvent OnVictory;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;
            for (int i = 0; i < enemiesPerWave; i++)
            {
                if (spawner.TrySpawnEnemy(out GameObject enemy))
                {
                    enemiesAlive++;
                    enemy.GetComponent<EnemyBase>().enabled = true;
                    enemy.GetComponent<EnemyBase>().GetComponent<EnemyShooter>().enabled = true;

                    enemy.GetComponent<EnemyBase>().GetComponent<EnemyBase>().TakeDamage(0); // Esto activa la referencia
                }
            }

            yield return new WaitUntil(() => enemiesAlive <= 0);
            yield return new WaitForSeconds(delayBetweenWaves);
        }

        OnVictory?.Invoke();
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }
}
