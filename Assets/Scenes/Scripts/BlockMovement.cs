using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class BlockMovement : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 1)]  [SerializeField]  float movementFactor;
    [SerializeField] float period = 1f;
    const float tau = Mathf.PI * 2f;
    Vector3 startPosition;
    

    void Start()
    {

        startPosition = transform.position;

    }


    void Update()
    {
        OscillateBlock();
    }

    private void OscillateBlock()
    {
        float cycles = Time.time / period;
        float rawSineWave = Mathf.Sin(tau * cycles);
        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementVector * movementFactor;

        if (period > Mathf.Epsilon)
        {
            transform.position = startPosition + offset;
        }
        else
        {
            return;
        }
          
    }
}
