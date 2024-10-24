using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public float gravityAcceleration = 9.81f; // Default to Earth's gravity
    private float terminalVelocity = 55f;
    public Transform originalPosition;
    public float resetDuration = 0.5f;
    public GravityPlanetsController gravityPlanets;

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isFalling;
    private bool finishedDropping;

    private Dictionary<string, float> planetGravities = new Dictionary<string, float>
    {
        { "Mercury", 2f },
        { "Venus", 15f },
        { "Earth", 9.81f },
        { "Mars", 1.5f },
        { "Jupiter", 40f },
        { "Saturn", 25f },
        { "Uranus", 5f },
        { "Neptune", 30f },
        { "Pluto", 0.1f }
    };

    private Dictionary<string, float> planetTerminalVelocities = new Dictionary<string, float>
    {
        { "Mercury", 20f },
        { "Venus", 100f },
        { "Earth", 55f },
        { "Mars", 10f },
        { "Jupiter", 200f },
        { "Saturn", 120f },
        { "Uranus", 30f },
        { "Neptune", 150f },
        { "Pluto", 5f }
    };


    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.rb = this.GetComponent<Rigidbody>();
        this.isFalling = false;
        this.finishedDropping = true;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            Vector3 gravity = new Vector3(0, -gravityAcceleration, 0);
            this.rb.AddForce(gravity * rb.mass, ForceMode.Force);

            // Cap the velocity to the terminal velocity
            if (this.rb.velocity.magnitude > terminalVelocity)
            {
                this.rb.velocity = Vector3.ClampMagnitude(this.rb.velocity, terminalVelocity);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!this.finishedDropping)
            {
                this.isFalling = false;
                this.finishedDropping = true;
                this.audioSource.Play();
            }

        }
    }

    public void DropApple()
    {
        string selectedPlanet = gravityPlanets.SelectedPlanet;
        if (planetGravities.ContainsKey(selectedPlanet) && planetTerminalVelocities.ContainsKey(selectedPlanet))
        {
            gravityAcceleration = planetGravities[selectedPlanet];
            terminalVelocity = planetTerminalVelocities[selectedPlanet];
        }
        else
        {
            gravityAcceleration = 9.81f;  // Default to Earth's gravity
            terminalVelocity = 55f;       // Default to Earth's terminal velocity
            Debug.Log("Planet not found, using default gravity and terminal velocity.");
        }

        this.isFalling = true;
        this.finishedDropping = false;
    }

    public void ResetApple()
    {
        this.isFalling = false;
        this.finishedDropping = true;
        this.rb.velocity = Vector3.zero;
        this.rb.angularVelocity = Vector3.zero;

        StartCoroutine(ResetApplePosition());
    }

    private IEnumerator ResetApplePosition()
    {
        Vector3 startPosition = this.transform.position;
        Quaternion startRotation = this.transform.rotation;
        Vector3 endPosition = originalPosition.position;
        Quaternion endRotation = originalPosition.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < resetDuration)
        {
            // Lerp position and rotation over time
            this.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / resetDuration);
            this.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / resetDuration);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final position and rotation are exactly at the original
        this.transform.position = endPosition;
        this.transform.rotation = endRotation;
    }
}
