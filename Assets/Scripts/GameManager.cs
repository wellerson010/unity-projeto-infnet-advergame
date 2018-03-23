using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Xml;

public class GameManager : MonoBehaviour
{
    public TextAsset LevelsXML;


    public float SpeedGame;
    public int TotalTracks;
    public int LimitTotalFoods;
    public int LimitFoodsGood;
    public int Life;
    public GameObject ContainerTrack;
    public GameObject Track;
    public GameType GameType;
    public int Level;

    [Header("GUI")]
    public Text TxtLevelNumber;

    private int TotalFoods;
    private IList<Track> Tracks;

    void Start()
    {
        TotalFoods = 0;
        Tracks = new List<Track>();

        ReadLevel();

        GenerateTracks();

        TxtLevelNumber.text = Level.ToString();
    }

    private void ReadLevel()
    {
        string text = LevelsXML.text;

        XmlDocument document = new XmlDocument();
        document.LoadXml(text);


    }



    private void GenerateTracks()
    {
        ClearGrid();

        float part = 1f / (float)TotalTracks;
        float half = part / 2;

        for (int i = 1; i <= TotalTracks; i++)
        {
            GameObject gameObject = Instantiate(Track);

            Track track = gameObject.GetComponent<Track>();
            track.Speed = SpeedGame;

            gameObject.transform.SetParent(ContainerTrack.transform);

            float positionY = -0.5f + (part * (i - 1)) + half;

            gameObject.transform.localPosition = new Vector3(0, positionY, 0);

            Tracks.Add(track);
        }
    }

    void ClearGrid()
    {
        foreach (Track track in Tracks)
        {
            Destroy(track.gameObject);
        }

        Tracks.Clear();
    }

    void Update()
    {
        DetectTapFood();

        switch (GameType)
        {
            case GameType.Limit:
                UpdateGameModeLimit();
                break;
        }
    }

    private void UpdateGameModeLimit()
    {
        foreach (Track track in Tracks)
        {
            if (track.CanInsertFood && TotalFoods <= LimitTotalFoods)
            {
                Food food = track.GetComponent<Track>().InsertFood();
                
                if (food != null)
                {
                    TotalFoods++;
                }
            }
        }
    }

    private void DetectTapFood()
    {
        for(int i=0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero);

                if (hitInfo && hitInfo.transform.gameObject.CompareTag("Food"))
                {
                    FoodClicked(hitInfo.transform.gameObject);
                }
            }
        }
    }

    public void FoodClicked(GameObject objectFood)
    {
        Food food = objectFood.GetComponent<Food>();

        bool foodNotCollided = false;

        if (!food.CollidedWithCollider)
        {
            foodNotCollided = true;
        }

        food.Track.DeleteFood(food);

        Destroy(objectFood);

        if (foodNotCollided)
        {
            food.Track.EnableInsertFood();
        }
    }
}
