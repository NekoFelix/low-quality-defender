using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private float delay = 2.5f;

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<ScoreState>().ResetScore();
    }

    public void LoadGameOverScene()
    {
        StartCoroutine(DelayLoadGameOverScene());
    }

    private IEnumerator DelayLoadGameOverScene()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("GameOver");
    }

    public void LoadFirstLevelScene()
    {
        SceneManager.LoadScene("Level01");
    }

    public void LoadNextScene()
    {
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
