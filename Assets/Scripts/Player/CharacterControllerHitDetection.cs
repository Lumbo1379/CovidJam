using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerHitDetection : MonoBehaviour
{
    [Range(0, 10)] public float CollidePower;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3)
            return;

        var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * CollidePower;
    }
}
