using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlanetsController : MonoBehaviour
{
    public List<GravityPlanet> planets;
    public Material selectedMaterial;
    public Material notSelectedMaterial;
    public string SelectedPlanet;

    private AudioSource audioSource;

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void SelectPlanet(GravityPlanet planet)
    {
        foreach (GravityPlanet p in this.planets)
        {
            if (p == planet)
            {
                p.isSelected = true;
                p.spotlight.intensity = 1.0f;
                this.SelectedPlanet = p.name;
                MeshRenderer meshRenderer = p.Circular_Light_01.GetComponent<MeshRenderer>();

                if (meshRenderer != null && meshRenderer.materials.Length > 0)
                {
                    Material[] materials = meshRenderer.materials;
                    materials[0] = selectedMaterial;
                    meshRenderer.materials = materials;
                    this.audioSource.Play();
                }
            }
            else
            {
                p.isSelected = false;
                p.spotlight.intensity = 0;
                MeshRenderer meshRenderer = p.Circular_Light_01.GetComponent<MeshRenderer>();

                if (meshRenderer != null && meshRenderer.materials.Length > 0)
                {
                    Material[] materials = meshRenderer.materials;
                    materials[0] = notSelectedMaterial;
                    meshRenderer.materials = materials;
                }
            }
        }


    }
}
