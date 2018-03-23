using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    private static GameObject SoundGameObject;

    private void Awake()
    {
        if (SoundGameObject == null)
        {
            SoundGameObject = gameObject;
            DontDestroyOnLoad(SoundGameObject);
        }
        else
        {
            Destroy(SoundGameObject);
        }
    }
}
