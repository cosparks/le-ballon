using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] TextMeshProUGUI tmPro;
    [SerializeField] int baseScreenWidth = 1920;

    [Header("Objects")]
    [SerializeField] Transform startPlatform;
    [SerializeField] Transform endPlatform;
    [SerializeField] Transform balloon;

    [Header("Music")]
    [SerializeField] StartMusic startMusic;
    [SerializeField] float loadDelay = 2f;

    private void Start()
    {
        SetFont();
        SetObjectXDisplacement();
    }

    private void SetFont()
    {
        int fontFactor = Screen.width - baseScreenWidth;


        if (fontFactor < 0)
        {
            tmPro.fontSize = (float)(tmPro.fontSize + fontFactor * 0.07);
        }
        else
        {
            tmPro.fontSize = (float)(tmPro.fontSize + fontFactor * 0.1);
        }
    }

    private void SetObjectXDisplacement()
    {
        int xFactor = Screen.width - baseScreenWidth;

        if (xFactor < 0)
        {
            xFactor = Mathf.Abs(xFactor);
            float leftX = -30.0f + 0.045f * xFactor;
            balloon.position = new Vector3(leftX, balloon.position.y, balloon.position.z);
            startPlatform.position = new Vector3(leftX, startPlatform.position.y, startPlatform.position.z);

            float rightX = 115f - 0.045f * xFactor; 
            endPlatform.position = new Vector3(rightX, endPlatform.position.y, endPlatform.position.z);
        }
    }

    public void StartGame()
    {
        startMusic.StartFade(loadDelay);
        Invoke("LoadGame", loadDelay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
