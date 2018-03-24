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
    public List<GameObject> StarsEnabled;

    void Start()
    {
        TextLevel.text = Info.Level.ToString();

        for(int i=0; i < StarsEnabled.Count; i++)
        {
            if (i < Info.Stars)
            {
                StarsEnabled[i].SetActive(true);
            }
            else
            {
                StarsEnabled[i].SetActive(false);
            }
        }
    }

    #if UNITY_EDITOR
    private void OnMouseDown()
    {
        Click();
    }
    #endif

    public void Click()
    {
        if (gameObject.GetComponent<Button>().interactable)
        {
            FindObjectOfType<SoundManager>().PlaySFXClick();
            GameObject.Find("MenuManager").GetComponent<LevelSelector>().GoToGame(Info);
        }
    }
}
