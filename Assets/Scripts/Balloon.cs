using System;
using UnityEngine;
using UnityEngine.SceneManagement;  // namespaces
using UnityStandardAssets.CrossPlatformInput;

public class Balloon : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    Display display;

    enum State { Alive, Dying, Transcending };
    State currentState = State.Alive;

    private float lastCollision = 0f;
    private bool playSFX;
    int sceneID;
    private bool collisionsEnabled = true;
    private int levelCount;
    float xThrow;

    [Header("Settings")]
    [SerializeField] int hitPoints = 10;
    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float upwardPull = 10f;
    [SerializeField] float loadDelay = 2f;
    [SerializeField] float timeBetweenCollisions = 1f;

    [Header("Sound FX")]
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [Header("Particle FX")]
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playSFX = false;
        sceneID = SceneManager.GetActiveScene().buildIndex;
        levelCount = SceneManager.sceneCountInBuildSettings - 1;

        display = FindObjectOfType<Display>();
        display.InitializeHealthDisplay(hitPoints);
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (currentState == State.Alive)
        {
            Thrust();
            //Rotation();
            NewRotation();
            UpwardForce();
        }
        if (Debug.isDebugBuild)
        {
            EnterDebugMode();
        }
    }

    private void NewRotation()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        transform.Rotate(-Vector3.forward * rotationThisFrame * xThrow);

    }

    void Rotation()

    {
        string rotation = "neutral";
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            rotation = "left";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotation = "right";
        }
        switch (rotation)
        {
            case "left":
                rigidBody.angularVelocity = Vector3.zero;
                transform.Rotate(Vector3.forward * rotationThisFrame);
                break;
            case "right":
                rigidBody.angularVelocity = Vector3.zero;
                transform.Rotate(-Vector3.forward * rotationThisFrame);
                break;
        }
    }

    void Thrust()
    {
        if (Input.GetButton("Fire"))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            Thrustsfx();
            mainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
            playSFX = false;
        }
    }

    void Thrustsfx()
    {
        if (playSFX == false)
        {
            audioSource.PlayOneShot(mainEngine);
            playSFX = true;
        }

    }

    void UpwardForce()
    {
        rigidBody.AddForce(Vector3.up * upwardPull);
    }

    private void EnterDebugMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive) { return; }

        else if (collisionsEnabled == true)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    FinishLevel();
                    break;
                case "Dead":  //same as enemy
                    RunDeathSequence();
                    break;
                case "Enemy": // same as dead
                    RunDeathSequence();
                    break;
                case "Wall":
                    HandleWallCollision();
                    break;
            }
        }
    }

    private void RunDeathSequence()
    {
        audioSource.Stop();
        deathParticles.Play();
        mainEngineParticles.Stop();
        currentState = State.Dying;
        Invoke("RestartScene", loadDelay);
        audioSource.PlayOneShot(death);
    }

    private void HandleWallCollision()
    {
        if (Time.time > lastCollision + timeBetweenCollisions)
        {
            hitPoints -= 1;
            display.SetHealthDisplay(hitPoints);
            lastCollision = Time.time;
        }

        if (hitPoints == 0)
        {
            RunDeathSequence();
        }
    }

    private void FinishLevel()
    {
        audioSource.Stop();
        display.DisplayPointsForLevel();

        currentState = State.Transcending;
        audioSource.PlayOneShot(success, 0.5f);
        successParticles.Play();
        Invoke("LoadNextScene", loadDelay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(sceneID);
    }

    private void LoadNextScene()
    {
        if (sceneID < levelCount)
        {
            SceneManager.LoadScene(sceneID + 1);
        }
        else if (sceneID == levelCount)
        {
            SceneManager.LoadScene(0);
        }
    }

    State GetCurrentState()
    {
        return currentState;
    }
}
