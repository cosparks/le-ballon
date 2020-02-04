using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    ProjectileLauncher projectileLauncherScript;
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;
    AudioSource audioSource;

    [SerializeField] AudioClip powerupSample;
    [SerializeField] [Range(0f, 1f)] float volume = 0.75f;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        projectileLauncherScript = FindObjectOfType<ProjectileLauncher>();
        projectileLauncherScript.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            projectileLauncherScript.enabled = true;
            Invoke("EndPowerup", projectileLauncherScript.GetPowerupTime());
            audioSource.PlayOneShot(powerupSample, volume);
            meshRenderer.enabled = false;
            boxCollider.enabled = false;
            Destroy(gameObject, 3f);
        }
    }

    private void EndPowerUp()
    {
        projectileLauncherScript.enabled = false;
    }
}
