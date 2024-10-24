using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanet : MonoBehaviour
{
    public GravityPlanetsController gravityPlanetsController;
    public Light spotlight;
    public GameObject Circular_Light_01;
    public bool isSelected;

    private Collider collider;

    private void Start()
    {
        this.isSelected = false;
        this.collider = this.GetComponent<Collider>();
        Transform spotlightTransform = transform.Find("Spot Light");

        if (spotlightTransform != null)
        {
            spotlight = spotlightTransform.GetComponent<Light>();

            if (spotlight != null)
            {
                spotlight.intensity = 0.0f;
            }
        }

        Transform Circular_Light_01Transform = transform.Find("Circular_Light_01");
        if (Circular_Light_01Transform != null)
        {
            Circular_Light_01 = Circular_Light_01Transform.gameObject;
        }
    }

    public void ClickOn()
    {
        if(!this.isSelected)
        {
            gravityPlanetsController.SelectPlanet(this);
        }
    }
}
