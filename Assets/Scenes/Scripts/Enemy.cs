using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script handles collisions, OnCollision sends out an isDead boolean to the enemies' physics scripts and a delayed isDead boolean
// to the targeting system of the ProjectileLauncher script (for a more dramatic, extended lightning animation)
public class Enemy : MonoBehaviour
{
    GameObject balloon;
    ProjectileLauncher balloonScript;
    float deathDelay;

    bool isDeadWithDelay;
    bool isDeadNow;

    private void Start()
    {
        isDeadWithDelay = false;
        GetDelayTime();
    }

    private void GetDelayTime()  // gets animationLength time from balloon script
    {
        balloon = GameObject.FindGameObjectWithTag("Target");
        balloonScript = balloon.GetComponentInChildren<ProjectileLauncher>();
        deathDelay = balloonScript.GetAnimationLength();
    }

    private void OnCollisionEnter(Collision collision)
    {
            Invoke("DelayedKillEnemy", deathDelay);
            isDeadNow = true;
    }

    private void DelayedKillEnemy()
    {
        isDeadWithDelay = true;
    }

    public bool IsEnemyDeadDelayed() // sent to Projectile Launcher script
    {
        return isDeadWithDelay;
    }

    public bool IsEnemyDeadNow() // sent to physics scripts
    {
        return isDeadNow;
    }
}
