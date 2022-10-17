using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    public int level, bestLevel;
    public GameObject nextButton, restartButton, panel, failPanel, confettyPrefab,jumpButton;
    public TextMeshProUGUI levelText, bestLevelText;
    public bool nLevel, fail,confetty;
    


    private void Awake()
    {
        gm = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        Load();
        nextButton.SetActive(false);
        restartButton.SetActive(false);
        failPanel.SetActive(false);
        nLevel = false;
        fail = false;
        confetty = false;
        jumpButton.SetActive(true);
        MoonSDK.TrackLevelEvents(MoonSDK.LevelEvents.Start, level);
    }

    // Update is called once per frame
    void Update()
    {
        if (level >= bestLevel)
        {
            bestLevelText.text = "BestLevel : " + level.ToString();

        }
        else
            bestLevelText.text = "BestLevel : " + bestLevel.ToString();

        levelText.text = "Level : " + level.ToString();

        if (nLevel)
        {
            if (!confetty)
            {
                Instantiate(confettyPrefab, PlayerControl.ctrl.transform.position, Quaternion.identity);
                confetty = true;
            }
            Invoke("NextButtonDelay", 2);
            panel.SetActive(false);
            jumpButton.SetActive(false);

        }
        if (fail)
        {
            jumpButton.SetActive(false);
            Invoke("RestartButtonDelay", 2);
            panel.SetActive(false);

        }
        Save();
    }


    private void NextButtonDelay()
    {
        nextButton.SetActive(true);

    }

    private void RestartButtonDelay()
    {
        failPanel.SetActive(true);

        restartButton.SetActive(true);

    }

    public void ButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ButtonNext()
    {
        level++;
        if (SceneManager.GetActiveScene().buildIndex >= 4)
        {
            SceneManager.LoadScene(1);
        }
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }


    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("bestLevel", bestLevel);
    }


    public void Load()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            level = PlayerPrefs.GetInt("level");
        }
        if (PlayerPrefs.HasKey("bestLevel"))
        {
            bestLevel = PlayerPrefs.GetInt("bestLevel");
        }
    }
}
