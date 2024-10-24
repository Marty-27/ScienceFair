using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Slider slider;
    private AudioSource audioSource;
    public AudioClip hoverSound;

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void StartScene()
    {
        int size = (int)this.slider.value;

        GameManager.Instance.WorldSize = size;
        SceneManager.LoadScene("Life");
    }

    public void HoverButton()
    {
        this.audioSource.clip = this.hoverSound;
        this.audioSource.Play();
    }

    public void Quit()
    {
        SceneManager.LoadScene("ScienceFair");
    }
}
