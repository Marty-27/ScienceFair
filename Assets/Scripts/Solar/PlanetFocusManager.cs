using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanetFocusManager : MonoBehaviour
{
    public static PlanetFocusManager Instance;

    public GameObject PlanetFocusArea;
    public GameObject SolarSystemArea;

    public TMP_Text PlanetNameText;
    public TMP_Text PlanetInfoText;

    public GameObject[] planets;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(gameObject);
        }
    }

    public void FocusOnPlanet(string planetName)
    {
        foreach(GameObject planet in planets)
        {
            planet.SetActive(false);
        }

        foreach (GameObject planet in planets)
        {
            if(planet.name == planetName)
            {
                planet.SetActive(true);
                break;
            }
        }

        this.PlanetNameText.text = planetName;
        this.LoadPlanetInfo(planetName);
    }

    private void LoadPlanetInfo(string planetName)
    {
        TextAsset infoText = Resources.Load<TextAsset>("PlanetInfo/" + planetName);
        if (infoText != null)
        {
            this.PlanetInfoText.text = infoText.text;
        }
        else
        {
            this.PlanetInfoText.text = "Information not available.";
        }
    }
}
