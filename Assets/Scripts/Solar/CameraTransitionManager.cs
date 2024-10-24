using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionManager : MonoBehaviour
{
    public static CameraTransitionManager Instance;

    public Transform XRRig;
    public Camera camera;
    public SolarSystemController solarSystemController;
    public SolarMovementProvider solarMovementProvider;

    public Transform SolarSystemCameraPosition;
    public Transform PlanetFocusCameraPosition;
    private AudioSource audioSource;

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

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }

    public void TransitionToPlanetFocus()
    {
        TransitionCamera(XRRig.transform, PlanetFocusCameraPosition);
        solarSystemController.isFocusing = true;
        solarSystemController.IsHovering = false;
        solarMovementProvider.upBtn.SetActive(false);
        solarMovementProvider.downBtn.SetActive(false);
    }

    public void TransitionToSolarSystem()
    {
        StartCoroutine(TransitionToSolarSystemCoroutine());
    }

    private IEnumerator TransitionToSolarSystemCoroutine()
    {
        // Keep isFocusing true during the transition
        solarSystemController.isFocusing = true;

        // Perform the teleportation
        TransitionCamera(XRRig.transform, SolarSystemCameraPosition);

        // Wait for end of frame or a small delay
        yield return new WaitForSeconds(1); // Alternatively, yield return null;

        // Now allow movement again
        solarSystemController.isFocusing = false;

        // Set up buttons and other UI elements
        if (XRRig.transform.position.y > 1)
        {
            solarMovementProvider.downBtn.SetActive(true);
        }
        solarMovementProvider.upBtn.SetActive(true);
    }

    private void TransitionCamera(Transform startTransform, Transform endTransform)
    {
        XRRig.position = endTransform.position;
        XRRig.rotation = endTransform.rotation;
        this.audioSource.Play();
    }
}
