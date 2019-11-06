using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    aTack_physics tackScript;
    attack_rocket rocketScript;
    GameObject balloon;
    ProjectileLauncher balloonScript;
    float deathDelay;

    bool isDead;

    private void Start()
    {
        isDead = false;
        rocketScript = GetComponent<attack_rocket>();
        tackScript = GetComponent<aTack_physics>();
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
        if (tackScript.IsTackDead() == true) // todo separate collision and controller scripts 
        {
            Invoke("KillEnemy", deathDelay);
            print("I'm a dead tack");
        }
        else if (rocketScript.IsRocketDead() == true)
        {
            Invoke("KillEnemy", deathDelay);
            print("I'm a dead rocket");
        }
        else
        {
            isDead = false;
        }
    }

    private void KillEnemy()
    {
        isDead = true;
    }

    public bool IsEnemyDead()
    {
        return isDead;
    }
}
