using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
	public float planetSpeedRotation = 1.0f;
	void Start()
	{

	}

	void LateUpdate()
	{

		transform.Rotate(-Vector3.up * Time.deltaTime * planetSpeedRotation);
	}
}
