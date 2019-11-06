using System;
using UnityEngine;
using UnityEngine.SceneManagement;  // namespaces
using UnityStandardAssets.CrossPlatformInput;

public class rocket_physics : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    private bool playSFX;
    int sceneID;
    private bool collisionsEnabled = true;
    private int levelCount;
    float xThrow;

    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float upwardPull = 10f;
    [SerializeField] float loadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    enum State { Alive, Dying, Transcending }
    State currentState = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playSFX = false;
        sceneID = SceneManager.GetActiveScene().buildIndex;
        levelCount = SceneManager.sceneCountInBuildSettings - 1;
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
        if (CrossPlatformInputManager.GetButton("Fire"))
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
                    audioSource.Stop();
                    currentState = State.Transcending;
                    audioSource.PlayOneShot(success, 0.7f);
                    successParticles.Play();
                    Invoke("LoadNextScene", loadDelay);
                    break;
                case "Dead":  //same as enemy
                    audioSource.Stop();
                    deathParticles.Play();
                    mainEngineParticles.Stop();
                    currentState = State.Dying;
                    Invoke("RestartScene", loadDelay);
                    audioSource.PlayOneShot(death);
                    break;
                case "Enemy": // same as dead
                    audioSource.Stop();
                    deathParticles.Play();
                    mainEngineParticles.Stop();
                    currentState = State.Dying;
                    Invoke("RestartScene", loadDelay);
                    audioSource.PlayOneShot(death);
                    break;
            }
        }
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
