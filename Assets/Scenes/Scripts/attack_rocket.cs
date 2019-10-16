using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack_rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    private bool playSFX;
    private bool playDeathSFX;

    Vector3 rotateAmount;
    [SerializeField] float attackDistance = 20f;

    [SerializeField] Transform rocketTarget;
    [SerializeField] float rotationThrust = 250f;
    [SerializeField] float engineThrust = 30f;

    [SerializeField] AudioClip mainEngine;
    [Range(0, 1)] [SerializeField] float engineVolume = 1f;
    [SerializeField] AudioClip death;
    [Range(0, 1)] [SerializeField] float deathVolume = 1f;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;

    enum State { Alive, Dead }
    State currentState = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rigidBody.useGravity = false;
        playSFX = false;
        playDeathSFX = false;
}

    void FixedUpdate()
    {
        if (attackDistance >= Vector3.Distance(rocketTarget.position, transform.position) && currentState == State.Alive)
        {
            TargetPlayer();
            AttackTarget();
        }
        else if (currentState == State.Dead && playDeathSFX == false)
        {
            audioSource.PlayOneShot(death, deathVolume);
            playDeathSFX = true;
        }
        else if (currentState == State.Alive)
        {
            playSFX = false;
            audioSource.Stop();
        }
        
    }

    private void AttackTarget()
    {
            Thrust();
            Direction();
            Thrustsfx();
    }

    private void Thrust()
    {
            rigidBody.useGravity = true;
            rigidBody.AddRelativeForce(Vector3.up * engineThrust);
            mainEngineParticles.Play();
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
        if (currentState != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Dead":
                audioSource.Stop();
                deathParticles.Play();
                mainEngineParticles.Stop();
                currentState = State.Dead;
                break;

            case "Target":
                audioSource.Stop();
                deathParticles.Play();
                mainEngineParticles.Stop();
                currentState = State.Dead;
                break;

        }
    }
}
