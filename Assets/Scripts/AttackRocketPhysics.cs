using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRocketPhysics : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    Enemy enemyScript;

    [SerializeField] float attackDistance = 20f;
    [SerializeField] GameObject explosionCollider;

    [Header("Physics Parameters")]
    Vector3 rotateAmount;
    [SerializeField] Transform rocketTarget;
    [SerializeField] float rotationThrust = 250f;
    [SerializeField] float engineThrust = 30f;

    [Header("Sound FX")]
    [SerializeField] AudioClip mainEngine;
    [Range(0, 1)] [SerializeField] float engineVolume = 1f;
    [SerializeField] AudioClip death;
    [Range(0, 1)] [SerializeField] float deathVolume = 1f;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    private bool playSFX;
    private bool playDeathSFX;

    bool isRocketDead;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        enemyScript = GetComponent<Enemy>();
        rigidBody.useGravity = false;
        playSFX = false;
        playDeathSFX = false;
        isRocketDead = false;
    }

    void FixedUpdate()
    {
        if (attackDistance >= Vector3.Distance(rocketTarget.position, transform.position) && isRocketDead == false)
        {
            TargetPlayer();
            AttackTarget();
        }
        else if (isRocketDead == true && playDeathSFX == false)
        {
            audioSource.PlayOneShot(death, deathVolume);
            playDeathSFX = true;
        }
        else if (isRocketDead == false)
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
        if (isRocketDead == true) { return; }

        isRocketDead = true;
        DeathSequence();
        CreateSplosionCollider();
    
    }

    private void DeathSequence()
    {
        rigidBody.useGravity = true;
        audioSource.Stop();
        deathParticles.Play();
        mainEngineParticles.Stop();
    }

    private void CreateSplosionCollider()
    {
        GameObject rocketExplosionCollider = Instantiate(explosionCollider, transform.position, Quaternion.identity);
        Destroy(rocketExplosionCollider, 0.1f);
    }

    public bool IsRocketDead()
    {
        return isRocketDead;
    }
}
