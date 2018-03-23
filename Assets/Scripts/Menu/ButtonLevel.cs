using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{

    [HideInInspector]
    public LevelInfo Info;

    public Text TextLevel;

    void Start()
    {
        TextLevel.text = Info.Level.ToString();
    }

    #if UNITY_EDITOR
    private void OnMouseDown()
    {
        GameObject.Find("MenuManager").GetComponent<LevelSelector>().GoToGame(Info);
    }
    #endif


}
