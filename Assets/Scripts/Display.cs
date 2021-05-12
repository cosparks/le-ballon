using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public const int SIZE_FACTOR_FOR_POINTS = 3;

    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Slider healthBar;

    private int scoreSoFar = 0;
    private int levelPoints;

    private void Awake()
    {
        int displayCount = FindObjectsOfType<Display>().Length;
        if (displayCount > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        textMesh.text = scoreSoFar.ToString();
    }

    public void SetLevelPoints(int levelPoints)
    {
        this.levelPoints = levelPoints;
    }

    public void InitializeHealthDisplay(int maxHP)
    {
        healthBar.maxValue = maxHP;
        healthBar.value = maxHP;
    }

    public void SetHealthDisplay(int hitPoints)
    {
        healthBar.value = hitPoints;
    }

    public void DisplayPointsForLevel()
    {
        scoreSoFar += levelPoints;
        textMesh.text = scoreSoFar.ToString();
    }
}