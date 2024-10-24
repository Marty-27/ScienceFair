using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SolarSystemController : MonoBehaviour
{
    public bool IsPaused = false;
    public bool IsPausedButton = true;
    public bool IsHovering = false;
    public bool isFocusing = false;

    public SolarMovementProvider solarMovementProvider;
    public CameraTransitionManager cameraTransitionManager;
    public FocusUIController focusUIController;
    public void toggleIsPausedButton()
    {
        this.IsPausedButton = !this.IsPausedButton;
    }

    public void Quitter()
    {
        SceneManager.LoadScene("ScienceFair");
    }

    public void Reset()
    {
        IsPaused = false;
        IsPausedButton = true;
        IsHovering = false;
        isFocusing = false;

        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
        foreach(GameObject planet in planets)
        {
            PlanetController planetController = planet.GetComponent<PlanetController>();
            planetController.Reset();
        }

        cameraTransitionManager.TransitionToSolarSystem();
        solarMovementProvider.downBtn.SetActive(false);

        focusUIController.Reset();
    }
}
