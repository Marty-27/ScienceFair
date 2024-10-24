using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class GravityMovementProvider : LocomotionProvider
{
    public float speed = 1.5f;
    public List<XRController> controllers = null;
    private CharacterController characterController;
    private XRRig rig;
    private Vector3 currentVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    private void Update()
    {
        if (controllers == null || controllers.Count == 0)
            return;

        Vector3 movementDirection = Vector3.zero;

        // Loop through controllers to get input
        foreach (var controller in controllers)
        {
            if (controller.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 inputAxis))
            {
                // Check if there is significant input
                if (inputAxis.magnitude > 0.1f)
                {
                    // Calculate movement based on touchpad input
                    Vector3 direction = new Vector3(inputAxis.x, 0, inputAxis.y);
                    direction = rig.cameraGameObject.transform.TransformDirection(direction);
                    movementDirection += direction;
                }
            }
        }

        // If there is movement input, update currentVelocity
        if (movementDirection != Vector3.zero)
        {
            movementDirection.Normalize();
            currentVelocity.x = movementDirection.x * speed;
            currentVelocity.z = movementDirection.z * speed;
        }
        else
        {
            // Reset horizontal movement when there's no input
            currentVelocity.x = 0;
            currentVelocity.z = 0;
        }

        // Move the character controller
        characterController.Move(currentVelocity * Time.deltaTime);
    }
}
