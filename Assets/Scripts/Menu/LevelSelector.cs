using Assets.Scripts.Model;
using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    public TextAsset LevelsXML;
    public GameObject ContainerLevels;
    public Button ButtonLevelSelector;

    [HideInInspector]
    public static LevelInfo LevelSelected;

    [HideInInspector]
    public static int LastLevelEnabled;

    [HideInInspector]
    public static IList<LevelInfo> Levels = new List<LevelInfo>();

    private XmlDocument Document = new XmlDocument();

    void Start()
    {
        LastLevelEnabled = 1;
        ReadXMLLevels();
    }

    void Update()
    {
        TouchService.DetectTouch(ClickLevelButton, true);
    }

    public void ClickLevelButton(RaycastHit2D hit)
    {
        GameObject gameObject = hit.transform.gameObject;
        if (gameObject.CompareTag("LevelButton"))
        {
            gameObject.GetComponent<ButtonLevel>().Click();
        }
    }

    public void GoToGame(LevelInfo info)
    {
        LevelSelected = info;
        SceneManager.LoadScene("Game");
    }

    private void ReadXMLLevels()
    {
        Document.LoadXml(LevelsXML.text);
        XmlNode nodeParent = Document.GetElementsByTagName("levels")[0];
        Levels.Clear();

        BuildUILevels(nodeParent.ChildNodes);
    }

    private void BuildUILevels(XmlNodeList nodes)
    {
        bool nextLevelEnabled = true;

        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Button button = Instantiate(ButtonLevelSelector, ContainerLevels.transform);

            int level = i + 1;

            int starHighScore = PlayerPrefsService.GetStarsHighScoreFromLevel(level);
            button.interactable = (nextLevelEnabled)?true:false;
            
            if (button.interactable)
            {
                LastLevelEnabled = level;
            }

            if (starHighScore > 0)
            {
                nextLevelEnabled = true;
            }
            else
            {
                nextLevelEnabled = false;
            }

            int tracks = Convert.ToInt32(node.SelectSingleNode("tracks").InnerText);
            int total = Convert.ToInt32(node.SelectSingleNode("total").InnerText);
            int good = Convert.ToInt32(node.SelectSingleNode("good").InnerText);
            int speed = Convert.ToInt32(node.SelectSingleNode("speed").InnerText);

            LevelInfo levelInfo = new LevelInfo()
            {
                Level = level,
                TotalFoods = total,
                TotalFoodsGood = good,
                Speed = speed,
                Tracks = tracks,
                Stars = starHighScore
            };

            Levels.Add(levelInfo);

            button.GetComponent<ButtonLevel>().Info = levelInfo;
        }
    }
    
}
