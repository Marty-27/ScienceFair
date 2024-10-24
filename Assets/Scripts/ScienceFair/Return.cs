using UnityEngine;
using UnityEngine.SceneManagement;

public class Return : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void goBack()
    {
        // Load science fair scene
        SceneManager.LoadScene("ScienceFair");
    }
}
