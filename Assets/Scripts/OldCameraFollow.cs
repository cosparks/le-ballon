using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldCameraFollow : MonoBehaviour
{
    [SerializeField] GameObject balloon;
    public float smoothspeed = 0.25f;
    public Vector3 offset;

    private Transform target;

    private void Start()
    {
        target = balloon.transform;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothspeed);
        transform.position = smoothedPosition;
    }
}
