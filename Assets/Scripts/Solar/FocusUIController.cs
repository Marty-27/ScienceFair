using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusUIController : MonoBehaviour
{
    public Slider LightingSlider;
    public Slider RotationSlider;
    public AudioSource AudioSource;
    public SolarSystemController solarSystemController;
    public Light light;

    private void Start()
    {
        this.AudioSource = this.GetComponent<AudioSource>();
    }

    public void HoverSound()
    {

        this.AudioSource.Play();

    }

    public void ChangeLighting()
    {
        light.intensity = this.LightingSlider.value;
    }

    public void Reset()
    {
        this.LightingSlider.value = 2;
        this.LightingSlider.value = 10;
    }
}
