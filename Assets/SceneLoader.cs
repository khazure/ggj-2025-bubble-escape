using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Changes the scene based on the sceneID available in the build list
    public void moveToScene(int sceneID) {
        SceneManager.LoadScene(sceneID);
    }

    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadWinScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
