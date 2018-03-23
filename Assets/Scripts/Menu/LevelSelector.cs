using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour {
    public TextAsset LevelsXML;
    public GameObject ContainerLevels;
    public Button ButtonLevelSelector;

    private XmlDocument Document = new XmlDocument();

    // Use this for initialization
    void Start()
    {
        ReadXMLLevels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ReadXMLLevels()
    {
        Document.LoadXml(LevelsXML.text);
        XmlNode nodeParent = Document.GetElementsByTagName("levels")[0];

        int totalLevels = nodeParent.ChildNodes.Count;
        BuildUILevels(totalLevels);



        foreach (XmlNode node in nodeParent.ChildNodes)
        {

        }
    }

    private void BuildUILevels(int totalLevels)
    {
        for (int i = 0; i < totalLevels; i++)
        {
            Button button = Instantiate(ButtonLevelSelector, ContainerLevels.transform);
        }
    }
}
