using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBucket : MonoBehaviour
{
    public Color bucketColor;
    public float PourRate = 1.0f;
    public ParticleSystem pouringEffect;
    public MixController mixController;

    private bool isPouring = false;
    private float pourDuration = 0f;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public Transform bigBucketTransform;
    private Vector3 pouringPosition;
    private Quaternion pouringRotation;

    private ColorMixing colorMixing;
    private AudioSource audioSource;
    private void Start()
    {
        this.originalPosition = this.transform.position;
        this.originalRotation = this.transform.rotation;

        Vector3 pouringPositionOffset = new Vector3(0, 1.25f, 0.5f);
        pouringPosition = bigBucketTransform.position + pouringPositionOffset;
        pouringRotation = Quaternion.Euler(-90f, 0f, 0f);

        this.colorMixing = bigBucketTransform.GetComponent<ColorMixing>();
        this.audioSource = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isPouring)
        {
            float amount = Time.deltaTime * PourRate;
            this.colorMixing.AddColor(bucketColor, amount);
        }
    }

    private void OnMouseDown()
    {
        this.OnClick();
    }

    public void OnClick()
    {
        if (!isPouring)
        {
            if(mixController.isPouring)
            {
                mixController.pouringPaintBucket.Reset();
            }
            mixController.pouringPaintBucket = this;
            mixController.isPouring = true;
            isPouring = true;

            StartCoroutine(MoveToPosition(pouringPosition, pouringRotation));
            var particleRenderer = pouringEffect.GetComponent<ParticleSystemRenderer>();
            particleRenderer.material.color = this.bucketColor;
            pouringEffect.Play();
            this.audioSource.Play();
        }
        else
        {
            isPouring = false;
            pouringEffect.Stop();

            StartCoroutine(MoveToPosition(originalPosition, originalRotation));

            ColorMixing mixing = bigBucketTransform.GetComponent<ColorMixing>();
            if (mixing != null)
            {
                mixing.AddColor(bucketColor, pourDuration);
            }

            pourDuration = 0f;
            this.audioSource.Stop();
        }
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition, Quaternion targetRotation)
    {
        float time = 0f;
        float duration = 0.3f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPosition, time / duration);
            transform.rotation = Quaternion.Lerp(startRot, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    public void Reset()
    {
        isPouring = false;
        pouringEffect.Stop();
        this.audioSource.Stop();

        StartCoroutine(MoveToPosition(originalPosition, originalRotation));

        pourDuration = 0f;
    }
}
