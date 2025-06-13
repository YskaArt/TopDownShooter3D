using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float duration;
    }
    public static event Action OnAllWavesCleared;

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
        StartCoroutine(BeginWithInitialDelay());
    }

    private void Update()
    {
        if (!isSpawning) return;

        waveTimer -= Time.deltaTime;
        UpdateHUD();

        // Limpia la lista de enemigos muertos
        activeEnemies.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

        // Final de oleada
        if ((waveTimer <= 0f || activeEnemies.Count == 0) && isSpawning)
        {
            isSpawning = false;
            StartCoroutine(StartNextWithDelay(3f));
        }
    }
    private IEnumerator BeginWithInitialDelay()
    {
        if (currentWaveIndex == 0)
        {
            yield return new WaitForSeconds(3f);
            StartCoroutine(ShowCenterMessage("¡Prepárate! La batalla comienza...", 2f));
            yield return new WaitForSeconds(2f);
        }
        StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            HandleVictoria();
            yield break;
        }

        Wave current = waves[currentWaveIndex];
        waveTimer = current.duration;

        waveText.text = $"Oleada {currentWaveIndex + 1}";
        StartCoroutine(ShowCenterMessage("¡Se avecina una oleada de enemigos!", 2f));

        for (int i = 0; i < current.enemyCount; i++)
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
    private IEnumerator StartNextWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentWaveIndex++;
        StartCoroutine(SpawnWave());
    }
    private IEnumerator ShowCenterMessage(string message, float duration)
    {
        centerMessageText.text = message;
        centerMessageText.enabled = true;
        yield return new WaitForSeconds(duration);
        centerMessageText.enabled = false;
    }
    private void HandleVictoria()
    {
        waveText.text = "¡Victoria!";
        timerText.text = "";
        enemyCountText.text = "";

        OnAllWavesCleared?.Invoke();

        Debug.Log("Juego terminado con éxito.");

        
    }
    private void UpdateHUD()
    {
        if (activeEnemies == null) return;

        enemyCountText.text = "Enemigos: " + activeEnemies.Count;

        if (currentWaveIndex >= waves.Count - 1)
        {
            timerText.text = "¡Última Oleada!";
        }
        else
        {
            timerText.text = "Siguiente Oleada en: " + Mathf.CeilToInt(waveTimer) + "s";
        }
    }
}
