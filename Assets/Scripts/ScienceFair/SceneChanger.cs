using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Name of the scene
    public string sceneName;

    void Start()
    {

    }

    void Update()
    {

    }

    public void loadScene()
    {
        // Change scene
        SceneManager.LoadScene(sceneName);
    }
}
