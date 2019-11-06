using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aTack_physics : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private bool playSFX;
    private bool playDeathSFX;
    private bool attackDanceComplete;

    const float tau = Mathf.PI * 2;
    private Vector3 startPosition;

    Vector3 rotateAmount;
    [SerializeField] float attackDistance = 20f;

    [SerializeField] Transform rocketTarget;
    [SerializeField] float rotationThrust = 250f;
    [SerializeField] float engineThrust = 30f;

    [SerializeField] Vector3 movementVector;
    [Range(0, 1)] [SerializeField] float movementFactor;
    [SerializeField] float period = 2f;
    [SerializeField] private float attackDanceTime = 2f;

    [SerializeField] AudioClip mainEngine;
    [Range(0, 1)] [SerializeField] float engineVolume = 1f;
    [SerializeField] AudioClip death;
    [Range(0, 1)] [SerializeField] float deathVolume = 1f;

    public bool isDead;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rigidBody.useGravity = false;
        playSFX = false;
        playDeathSFX = false;
        startPosition = transform.position;
        isDead = false;
    }

    void FixedUpdate()
    {
        if (attackDistance >= Vector3.Distance(rocketTarget.position, transform.position) && isDead == false)
        {
            // AttackDance();
            TargetPlayer();
            AttackTarget();
        }
        else if (isDead == true && playDeathSFX == false)
        {
            rigidBody.useGravity = true;
            audioSource.PlayOneShot(death, deathVolume);
            playDeathSFX = true;
        }
        else if (isDead == false)
        {
            audioSource.Stop();
            playSFX = false;
        }
    }

    private void AttackDance()
    {
        float cycles = Time.time / period;
        float rawSineWave = Mathf.Sin(tau * cycles);
        movementFactor = rawSineWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startPosition + offset;
    }

    private void AttackTarget()
    {
        Thrust();
        Direction();
    }

    private void Thrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * engineThrust);
        Thrustsfx();
        Wobble();
    }

    private void Wobble()
    {
        
    }

    private void Direction()
    {
        float rotationThisFrame = rotationThrust * Time.deltaTime;
        rigidBody.angularVelocity = -rotateAmount * rotationThisFrame;
    }

    private void TargetPlayer()
    {
        Vector3 targetDirection = rocketTarget.position - transform.position;
        targetDirection.Normalize();
        rotateAmount = Vector3.Cross(targetDirection, transform.up);
    }

        void Thrustsfx()
    {
        if (playSFX == false)
        {
            audioSource.PlayOneShot(mainEngine, engineVolume);
            playSFX = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead == true) { return; }

        switch (collision.gameObject.tag)
        {
            case "Dead":
                audioSource.Stop();
                isDead = true;
                break;
            case "Enemy":
                audioSource.Stop();
                isDead = true;
                break;
            case "Target":
                audioSource.Stop();
                isDead = true;
                break;

        }
    }
    public bool IsTackDead()
    {
        return isDead;
    }
}

