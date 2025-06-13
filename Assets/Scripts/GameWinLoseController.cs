using UnityEngine;
using System.Collections;
using System;

public class GameWinLoseController : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private float delay = 5f;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void OnEnable()
    {
        PlayerHealth.OnPlayerDied += OnPlayerDied;
        WaveManager.OnAllWavesCleared += OnAllWavesCleared;
    }
    private void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= OnPlayerDied;
        WaveManager.OnAllWavesCleared -= OnAllWavesCleared;
    }
    private void OnPlayerDied()
    {
        StartCoroutine(ShowScreenAfterDelay(loseScreen, delay));
    }
    private void OnAllWavesCleared()
    {
        StartCoroutine(ShowScreenAfterDelay(winScreen, delay));
    }
    private IEnumerator ShowScreenAfterDelay(GameObject screen, float seconds)
    {
       
        Cursor.visible = true;
        yield return new WaitForSeconds(seconds);
        screen.SetActive(true);
        Time.timeScale = 0;
    }
}
