using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using DigitalRuby.LightningBolt;

public class ProjectileLauncher : MonoBehaviour
{
    LightningBoltScript animatedLightning;
    GameObject[] enemies;           //create array of all targets
    GameObject closestTarget;       //closest enemy --> sent to LightningBolt script


    [Header("Game Object")]
    [SerializeField] GameObject lightningCollider;
    [SerializeField] Transform spawnedAtRuntime;

    [Header("Lightning Settings")]
    [SerializeField] float minAttackDistance = 20f;
    [Tooltip("Changes location of lightning hitbox relative to target")] [SerializeField] Vector3 lightningHitOffset;
    [Tooltip("Number of frames that lightning can be active before cooldown")] [SerializeField] int frameLimit = 100;
    [Tooltip("Determines how long lightning animation will be active")] [SerializeField] float animationLength = 0.4f;
    [Tooltip("Number of seconds before lightning can be used again")] [SerializeField] float coolDown = 2f;
    [Tooltip("Number of seconds before lightning powerup wears off")] [SerializeField] float powerupTimeLimit = 10f;
    [SerializeField] bool hilariousBug; // activates hilarious bug
    float attackTime;
    bool attackCalled;
    int frameCounter;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");               // creates array of enemies
        animatedLightning = GetComponentInChildren<LightningBoltScript>();  // accesses lightning generator
        closestTarget = enemies[0];                                         // updated by GetClosestEnemy()
        print("There are " + enemies.Length + " enemies in this level");
    }

    private void Update()
    {
        if (CrossPlatformInputManager.GetButton("Shock"))
        {
            GetClosestEnemy(enemies);

            if (!hilariousBug)
            {
                LightningAttack();
            }
            else if (hilariousBug)
            {
                BuggyLightningAttack();
            }
        }
        else
        {
            animatedLightning.Trigger(false);
        }
    }

    GameObject GetClosestEnemy(GameObject[] baddies)
    {
        closestTarget = enemies[0];
        float minDist = minAttackDistance;
        Vector3 currentPos = transform.position;
        foreach (GameObject baddy in baddies)
        {
            float dist = Vector3.Distance(baddy.transform.position, currentPos);
            Enemy enemyScript = baddy.GetComponent<Enemy>();
            if (!hilariousBug)
            {
                if (dist < minDist && enemyScript.IsEnemyDeadDelayed() == false)
                {
                    closestTarget = baddy;
                    minDist = dist;
                }
            }
            else if (hilariousBug)
            {
                if (dist < minDist)
                {
                    closestTarget = baddy;
                    minDist = dist;
                }
            }
        }
        return closestTarget;
    }

    private void LightningAttack()  //todo Clean up this method
    {
        if (Vector3.Distance(gameObject.transform.position, closestTarget.transform.position) <= minAttackDistance  // makes sure target is within range
            && frameCounter <= frameLimit                                                                              // there is still attack energy/ammo
            && Time.time >= (attackTime + coolDown)                                                                    // cooldown must be finished
            )
        {
            attackCalled = true;
            frameCounter++;
            animatedLightning.Trigger(true);    // triggers lightning
            KillTarget();                       // generates collider in transform position of target
        }

        // extremely silly way of EXTENDING LIGHTNING ANIMATION TIME
        else if (Vector3.Distance(gameObject.transform.position, closestTarget.transform.position) <= minAttackDistance
                && frameCounter <= frameLimit
                && (attackTime + animationLength) >= Time.time
                )
        {
            animatedLightning.Trigger(true);
            KillTarget();
        }

        ProcessAttackTime();
    }

    private void ProcessAttackTime()
    {
        if (attackCalled)
        {
            attackTime = Time.time;
            attackCalled = false;
        }
    }

    private void KillTarget()
    {
        GameObject lightningHit = Instantiate(lightningCollider, closestTarget.transform.position + lightningHitOffset, Quaternion.identity);
        lightningHit.transform.parent = spawnedAtRuntime;
        Destroy(lightningHit, 0.05f);
    }

    public float GetAttackDistance()
    {
        return minAttackDistance;
    }

    public GameObject GetClosestTarget()
    {
        return closestTarget;
    }

    public float GetAnimationLength()
    {
        return animationLength;
    }

    public float GetPowerupTime()
    {
        return powerupTimeLimit;
    }

    // extra-buggy lightning attack just for fun :)
    private void BuggyLightningAttack()
    {
        if (Vector3.Distance(gameObject.transform.position, closestTarget.transform.position) <= minAttackDistance)
        {
            animatedLightning.Trigger(true);    // triggers lightning
            KillTarget();
        }
    }
}
