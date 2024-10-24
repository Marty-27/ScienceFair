using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlanetController : MonoBehaviour
{
	public float planetSpeedRotation = 1.0f;
	public Slider rotationSlider;
	void Start()
	{

	}

	void LateUpdate()
	{

		transform.Rotate(-Vector3.up * Time.deltaTime * planetSpeedRotation);
	}

	public void ChangeRotationSpeed()
    {
		this.planetSpeedRotation = rotationSlider.value;
    }
}
