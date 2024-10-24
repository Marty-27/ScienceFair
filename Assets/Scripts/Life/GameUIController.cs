using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    public GameObject cube;
    private AudioSource audioSource;
    public AudioClip upSound;
    public AudioClip downSound;
    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }
    public void IncreaseCubeSize()
    {
        this.audioSource.clip = upSound;
        this.audioSource.Play();
        Vector3 newScale = cube.transform.localScale;
        newScale.x += 0.5f;
        newScale.y += 0.5f;
        newScale.z += 0.5f;
        cube.transform.localScale = newScale;
    }

    public void DecreaseCubeSize()
    {
        Vector3 newScale = cube.transform.localScale;

        if (newScale.x > 0.5f && newScale.y > 0.5f && newScale.z > 0.5f)
        {
            this.audioSource.clip = downSound;
            this.audioSource.Play();
            newScale.x -= 0.5f;
            newScale.y -= 0.5f;
            newScale.z -= 0.5f;
            cube.transform.localScale = newScale;
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Life Menu");
    }
}
