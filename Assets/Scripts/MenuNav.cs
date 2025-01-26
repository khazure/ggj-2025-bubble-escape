using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuNav : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changes the scene based on the sceneID available in the build list
    public void moveToScene(int sceneID) {
        SceneManager.LoadSceneAsync(sceneID);
    }

    public void quitGame() {
        Application.Quit();
    }

}
