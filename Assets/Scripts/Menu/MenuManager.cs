using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    // This function will be called when the "Start" button is clicked.
    public void StartGame()
    {
        StartCoroutine(LoadSceneWithDelay(1, 0.3f)); // Load scene index 1 after 2 seconds
    }

    IEnumerator LoadSceneWithDelay(int sceneIndex, float delay) //Delay
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    // This function will be called when the "Quit" button is clicked.
    public void QuitGame()
    {
        StartCoroutine(QuitWithDelay(0.3f)); // Quit after 2 seconds
    }

    IEnumerator QuitWithDelay(float delay)   //Delay
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    //Method to go to home button.
    public void GoToHome()
    {
        StartCoroutine(LoadSceneWithDelay(0, 0.3f));
    }
}
