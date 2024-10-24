using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isFalling;
    private bool canCreateLive;
    private float timeSinceLastCheck;
    private float collisionCheckInterval = 0.5f;
    private bool canCheckCollision;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private AudioSource audioSource;

    public WallGenerator wallGenerator;
    public float pushForce = 15f;
    public AudioClip dropSound;
    public AudioClip clickSound;
    public XRController controller;
    private void Start()
    {
        this.originalPosition = this.transform.position;
        this.originalRotation = this.transform.rotation;
        this.rb = this.GetComponent<Rigidbody>();
        this.isFalling = false;
        this.timeSinceLastCheck = 0f;
        this.canCheckCollision = false;
        this.audioSource = this.GetComponent<AudioSource>();
        this.canCreateLive = true;
    }

    private void Update()
    {
        this.timeSinceLastCheck += Time.deltaTime;

        if (this.timeSinceLastCheck >= this.collisionCheckInterval)
        {
            this.timeSinceLastCheck = 0f;
            this.canCheckCollision = true;
        }
        if (this.rb.velocity.y < -0.1f && !this.isFalling && (this.transform.position.y > (1 + this.transform.localScale.y / 2)))
        {
            this.isFalling = true;
        }
        
    }

    public void PushCube()
    {
        Vector3 controllerPosition = controller.transform.position;
        Vector3 direction = (transform.position - controllerPosition).normalized; // Push based on controller position
        this.rb.AddForce(direction * pushForce, ForceMode.Impulse);
        this.audioSource.clip = this.clickSound;
        this.audioSource.Play();
        if (!this.canCreateLive)
        {
            this.canCreateLive = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.canCheckCollision && this.canCreateLive)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                this.canCreateLive = false;
                if (this.isFalling)
                {
                    this.audioSource.clip = this.dropSound;
                    this.audioSource.Play();
                    this.isFalling = false;
                }

                this.canCheckCollision = false;
                WallCellInfo cellInfo = collision.gameObject.GetComponent<WallCellInfo>();
                if (cellInfo != null)
                {
                    int minX = int.MaxValue, maxX = int.MinValue;
                    int minY = int.MaxValue, maxY = int.MinValue;
                    foreach (ContactPoint contact in collision.contacts)
                    {
                        Vector3 localPoint = collision.transform.InverseTransformPoint(contact.point);

                        int x, y;
                        if (cellInfo.GetCellAtPoint(localPoint, out x, out y))
                        {
                            if (x < minX) minX = x;
                            if (x > maxX) maxX = x;
                            if (y < minY) minY = y;
                            if (y > maxY) maxY = y;
                        }
                    }

                    for (int x = minX; x <= maxX; x++)
                    {
                        for (int y = minY; y <= maxY; y++)
                        {
                            wallGenerator.SetCellState(collision.gameObject.name, x, y, true);
                        }
                    }
                }
                this.wallGenerator.start = true;
            }
        }
    }

    public void Reset()
    {
        this.transform.position = this.originalPosition;
        this.transform.rotation = this.originalRotation;
        this.isFalling = false;
        this.timeSinceLastCheck = 0f;
        this.canCheckCollision = false;
        this.canCreateLive = true;
    }
}
