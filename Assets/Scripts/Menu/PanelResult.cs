using Assets.Scripts.Enum;
using Assets.Scripts.Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelResult : MonoBehaviour
{
    public Text TextResult;
    public Color BadResult;
    public Color OKResult;
    public Color GoodResult;
    public Color AwesomeResult;
    public string BadResultText;
    public string OKResultText;
    public string GoodResultText;
    public string AwesomeResultText;
    public List<GameObject> StarsEnabled;
    public GameObject ButtonNext;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevelMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        LevelSelector.LevelSelected = LevelSelector.Levels[LevelSelector.LevelSelected.Level];
        Restart();
    }

    public void SetScore(Score score, int level)
    {
        ButtonNext.SetActive(false);
        gameObject.SetActive(true);
        int stars = 0;

        switch (score)
        {
            case Score.Bad:
                TextResult.text = BadResultText;
                TextResult.color = BadResult;
                FindObjectOfType<SoundManager>().PlaySFXLoose();
                break;
            case Score.Ok:
                stars = 1;
                TextResult.text = OKResultText;
                TextResult.color = OKResult;
                FindObjectOfType<SoundManager>().PlaySFXVictory();
                break;
            case Score.Good:
                stars = 2;
                TextResult.text = GoodResultText;
                TextResult.color = GoodResult;
                FindObjectOfType<SoundManager>().PlaySFXVictory();
                break;
            case Score.Awesome:
                stars = 3;
                TextResult.text = AwesomeResultText;
                TextResult.color = AwesomeResult;
                FindObjectOfType<SoundManager>().PlaySFXAwesomeVictory();
                FindObjectOfType<SoundManager>().PlaySFXVictory();
                break;
        }

        SetStarsEnabled(stars);

        if (stars > 0 || LevelSelector.LastLevelEnabled > level)
        {
            if (LevelSelector.Levels.Count >= level + 1)
            {
                ButtonNext.SetActive(true);
            }
        }

        int starsHighScore = PlayerPrefsService.GetStarsHighScoreFromLevel(level);

        if (stars > starsHighScore)
        {
            PlayerPrefsService.SetStarsHighScoreFromLevel(stars, level);
        }
    }

    private void SetStarsEnabled(int total)
    {
        for (int i = 0; i < StarsEnabled.Count; i++)
        {
            if (i < total)
            {
                StarsEnabled[i].SetActive(true);
            }
            else
            {
                StarsEnabled[i].SetActive(false);
            }
        }
    }
}
