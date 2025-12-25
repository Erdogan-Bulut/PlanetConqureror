using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyundan çıkış yapıldı.");
    }
}