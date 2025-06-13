using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
    public void Quit()
    {
        Application.Quit();
    }
}
