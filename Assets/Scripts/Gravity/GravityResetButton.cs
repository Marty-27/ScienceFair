using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityResetButton : MonoBehaviour
{
    public AppleController appleController;
    private Renderer renderer;
    private Material material;
    private AudioSource audioSource;

    private void Start()
    {
        this.renderer = this.GetComponent<Renderer>();
        this.material = this.renderer.material;
        this.audioSource = this.GetComponent<AudioSource>();
    }
    public void ClickOn()
    {
        this.audioSource.Play();
        appleController.ResetApple();
    }

    public void HoverOn()
    {
        this.material.EnableKeyword("_EMISSION");
    }

    public void HoverExit()
    {
        this.material.DisableKeyword("_EMISSION");
    }
}
