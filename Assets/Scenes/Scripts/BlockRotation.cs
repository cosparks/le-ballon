using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotation : MonoBehaviour
{
    [SerializeField] Vector3 rotationSpeed;
    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period = 2f;
    const float tau = Mathf.PI * 2;

    Vector3 startPosition;


    void Start()
    {
        startPosition = transform.position;
    }


    void Update()
    {
        RotateObject();
        oscillateObject();
    }

    private void RotateObject()
    {
        transform.Rotate(rotationSpeed);
    }

    private void oscillateObject()
    {
        if (period > Mathf.Epsilon)
        {
            float cycles = Time.time / period;
            float rawSineWave = Mathf.Sin(tau * cycles);
            movementFactor = rawSineWave / 2f + 0.5f;
            Vector3 offset = movementFactor * movementVector;
            transform.position = startPosition + offset;
        }

        else
        {
            return;
        }
    }
}
