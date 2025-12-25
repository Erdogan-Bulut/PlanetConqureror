using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Death Screen Settings")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private CanvasGroup deathScreenGroup;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("Victory Screen")]
    [SerializeField] private GameObject victoryScreen;

    public void WinGame()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void GameOver()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0;
        deathScreenGroup.alpha = 0;
        StartCoroutine(FadeInUI());
    }

    private IEnumerator FadeInUI()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            deathScreenGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }
        deathScreenGroup.alpha = 1;

        Time.timeScale = 0; 
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}