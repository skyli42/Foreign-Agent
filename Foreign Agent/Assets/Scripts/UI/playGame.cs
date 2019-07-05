using UnityEngine;
using UnityEngine.SceneManagement;

public class playGame : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1.0f;
    }

    public void NextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        LoadingScreen.Instance.Show(SceneManager.LoadSceneAsync(sceneIndex));
        Time.timeScale = 1.0f;
    }
}
