using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    public float timeForFullCircle = 10f;
    public SolarSystemController solarSystemController;
    public PlanetFocusManager planetFocusManager;
    public CameraTransitionManager cameraTransitionManager;

    public float HoverScaleFactor = 1.2f;
    public float hoverAnimationDuration = 0.3f; // duration of hover animation

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float anglePerFrame;
    private bool isHovering = false;
    private float hoverTolerance;
    private Vector3 originalScale;
    private Coroutine hoverCoroutine;
    private AudioSource audioSource;
    private TrailRenderer trail;

    private void Start()
    {
        this.originalPosition = this.transform.position;
        this.originalRotation = this.transform.rotation;

        this.hoverTolerance = this.transform.localScale.x;
        this.audioSource = this.GetComponent<AudioSource>();

        this.originalScale = this.transform.localScale;
        if (this.transform.localScale.x <= 10)
        {
            this.hoverTolerance = this.transform.localScale.x + 5f;
            this.HoverScaleFactor += 5;
        }
        else
        {
            this.hoverTolerance = this.transform.localScale.x;
            this.HoverScaleFactor = 1.2f;
        }
        this.trail = this.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!solarSystemController.IsPaused && !solarSystemController.IsPausedButton)
        {
            this.anglePerFrame = 360f / timeForFullCircle * Time.deltaTime;
            this.transform.RotateAround(Vector3.zero, Vector3.down, this.anglePerFrame);
        }
    }

    private void StartHoverEffect()
    {

        if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(AnimateHoverEffect(originalScale, originalScale * HoverScaleFactor));
    }

    private void StopHoverEffect()
    {
        if (hoverCoroutine != null) StopCoroutine(hoverCoroutine);
        hoverCoroutine = StartCoroutine(AnimateHoverEffect(transform.localScale, originalScale));
    }

    private IEnumerator AnimateHoverEffect(Vector3 fromScale, Vector3 toScale)
    {
        float elapsedTime = 0f;

        while (elapsedTime < hoverAnimationDuration)
        {
            transform.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / hoverAnimationDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // wait for the next frame
        }

        // Ensure final scale is set
        transform.localScale = toScale;
    }

    public void TurnOnCurrentlyHovering()
    {
        if (!this.isHovering && !solarSystemController.IsHovering)
        {
            this.isHovering = true;
            solarSystemController.IsPaused = true;
            solarSystemController.IsHovering = true;
            this.audioSource.Play();
            StartHoverEffect();
        }
    }

    public void TurnOffCurrentlyHovering()
    {
        if (this.isHovering)
        {
            this.isHovering = false;
            solarSystemController.IsPaused = false;
            StopHoverEffect();
            solarSystemController.IsHovering = false;
        }
    }

    public void ClickOn()
    {
        solarSystemController.isFocusing = true;
        planetFocusManager.FocusOnPlanet(gameObject.name);
        cameraTransitionManager.TransitionToPlanetFocus();
    }

    public void Reset()
    {
        this.transform.position = this.originalPosition;
        this.transform.rotation = this.originalRotation;
        this.transform.localScale = this.originalScale;
        this.isHovering = false;
        if(trail != null)
        {
            trail.Clear();
        }
    }
}
