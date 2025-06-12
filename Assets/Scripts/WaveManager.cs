using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float duration;
    }

    [Header("Wave Settings")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("HUD References")]
    [SerializeField] private Text waveText;
    [SerializeField] private Text enemyCountText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text centerMessageText;

    private int currentWaveIndex = 0;
    private float waveTimer;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning = false;

   
    private void Start()
    {

        StartCoroutine(BeginWaveWithInitialDelay());
    }

    private void Update()
    {
        if (!isSpawning) return;

        waveTimer -= Time.deltaTime;
        UpdateHUD();

       
        activeEnemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

        // Condición de fin de ola
        if ((waveTimer <= 0f || activeEnemies.Count == 0) && currentWaveIndex < waves.Count)
        {
            isSpawning = false;
            StartCoroutine(StartNextWaveWithDelay(3f));
        }

        // ✅ Victoria al final de la última oleada y sin enemigos
        if (currentWaveIndex >= waves.Count && activeEnemies.Count == 0)
        {
            isSpawning = false;
            waveText.text = "¡Victoria!";
            timerText.text = "";
            enemyCountText.text = "";
            Debug.Log("Juego terminado con éxito.");
        }
    }
    private IEnumerator BeginWaveWithInitialDelay()
    {
        if (currentWaveIndex == 0)
        {
            yield return new WaitForSeconds(3f); // Tiempo de espera antes de la primera oleada
            StartCoroutine(ShowCenterMessage("¡Prepárate! La batalla comienza...", 2f));
            yield return new WaitForSeconds(2f);
        }

        StartCoroutine(SpawnWave());
    }


    private IEnumerator SpawnWave()
    {
        
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Todas las oleadas completadas. Esperando eliminación de enemigos.");
            isSpawning = true; // 🔁 Esto permite que el Update siga limpiando la lista
            yield break;
        }

        Wave currentWave = waves[currentWaveIndex];
        waveTimer = currentWave.duration;

       
            waveText.text = $"Oleada {currentWaveIndex + 1}";
        StartCoroutine(ShowCenterMessage("¡Se avecina una oleada de enemigos!", 2f));

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            GameObject enemy;
            if (enemySpawner.TrySpawnEnemy(out enemy))
            {
                activeEnemies.Add(enemy);
            }
        }

        isSpawning = true;
        UpdateHUD();
        yield return null;
    }
    private IEnumerator ShowCenterMessage(string message, float duration)
    {
        centerMessageText.text = message;
        centerMessageText.enabled = true;
        yield return new WaitForSeconds(duration);
        centerMessageText.enabled = false;
    }


    private IEnumerator StartNextWaveWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWaveIndex++;
        StartCoroutine(SpawnWave());
    }

    private void UpdateHUD()
    {
        enemyCountText.text = "Enemigos: " + activeEnemies.Count;

        if (currentWaveIndex >= waves.Count - 1)
        {
            timerText.text = "¡Última Oleada!";
        }
        else
        {
            timerText.text = "Siguiente Oleada En: " + Mathf.CeilToInt(waveTimer) + "s";
        }
    }
}
