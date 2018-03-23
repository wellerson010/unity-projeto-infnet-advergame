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

    private XmlDocument Document = new XmlDocument();

    void Start()
    {
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
            GoToGame(gameObject.GetComponent<ButtonLevel>().Info);
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

        BuildUILevels(nodeParent.ChildNodes);
    }

    private void BuildUILevels(XmlNodeList nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            XmlNode node = nodes[i];
            Button button = Instantiate(ButtonLevelSelector, ContainerLevels.transform);

            int tracks = Convert.ToInt32(node.SelectSingleNode("tracks").Value);
            int total = Convert.ToInt32(node.SelectSingleNode("total").Value);
            int good = Convert.ToInt32(node.SelectSingleNode("good").Value);

            LevelInfo levelInfo = new LevelInfo()
            {
                Level = i+1,
                TotalFoods = total,
                TotalFoodsGood = good
            };

            button.GetComponent<ButtonLevel>().Info = levelInfo;
        }
    }
}
