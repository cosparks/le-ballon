using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the behavior of the enemy tack
public class TackPhysics : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;
    Enemy enemyScript;
    private bool playSFX;
    private bool playDeathSFX;

    Vector3 rotateAmount;
  
    [Header("Movement Settings")]
    [SerializeField] float attackDistance = 20f;
    [SerializeField] Transform target;
    [SerializeField] float rotationThrust = 250f;
    [SerializeField] float engineThrust = 30f;


    [Header("Sound FX")]
    [SerializeField] AudioClip mainEngine;
    [Range(0, 1)] [SerializeField] float engineVolume = 1f;
    [SerializeField] AudioClip death;
    [Range(0, 1)] [SerializeField] float deathVolume = 1f;

    public bool isTackDead;
    public bool hasTarget = false;

    void Start()
    {
        if (target != null)
        {
            hasTarget = true;
        }
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        enemyScript = GetComponent<Enemy>();
        rigidBody.useGravity = false;
        playSFX = false;
        playDeathSFX = false;
        isTackDead = false;
    }

    void FixedUpdate()
    {
        if (hasTarget && attackDistance >= Vector3.Distance(target.position, transform.position) && isTackDead == false)
        {
            TargetPlayer();
            AttackTarget();
        }
        else if (isTackDead == true && playDeathSFX == false)
        {
            rigidBody.useGravity = true;
            audioSource.PlayOneShot(death, deathVolume);
            playDeathSFX = true;
        }
        else if (isTackDead == false)
        {
            audioSource.Stop();
            playSFX = false;
        }
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
    }

    private void Direction()
    {
        float rotationThisFrame = rotationThrust * Time.deltaTime;
        rigidBody.angularVelocity = -rotateAmount * rotationThisFrame;
    }

    private void TargetPlayer()
    {
        Vector3 targetDirection = target.position - transform.position;
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
        if (isTackDead == true) { return; }
        // TOP OF NEW SCRIPT -- DELETE IF BEHAVIOR BROKEN
        if (enemyScript.IsEnemyDeadNow() == true)
        {
            isTackDead = true;
            audioSource.Stop();
        }
        // BOTTOM OF NEW SCRIPT -- DELETE IF BEHAVIOR BROKEN
        // switch (collision.gameObject.tag)
        // {
        //     case "Dead":
        //        audioSource.Stop();
        //        isTackDead = true;
        //        break;
        //    case "Enemy":
        //        audioSource.Stop();
        //        isTackDead = true;
        //        break;
        //    case "Target":
        //        audioSource.Stop();
        //        isTackDead = true;
        //        break;
        // }
    }
    public bool IsTackDead()
    {
        return isTackDead;
    }

    public void SetAttackDistance(float attackDistance)
    {
        this.attackDistance = attackDistance;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        hasTarget = true;
    }
}

