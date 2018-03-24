using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Xml;
using Assets.Scripts.Model;
using Assets.Scripts.Enum;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int ErrorsAllowed;
    public GameObject ContainerTrack;
    public GameObject Track;
    public GameObject PanelResult;
    public GameType GameType;


    [Header("GUI")]
    public Text TxtLevelNumber;

  
    private float SpeedGame;
    private int Level;
    private int TotalTracks;
    private int LimitTotalFoods;
    private int LimitFoodsGood;
    private LevelInfo LevelInfo;
    private bool GameFinished;
    private int TotalFoodsCreated;
    private int TotalGoodFoodsClicked;
    private int TotalBadFoodsClicked;
    private IList<Track> Tracks;
    private IList<Food> Foods;
    private IList<FoodType> FoodsToCreated;
    private bool GameOver;

    void Start()
    {
        Foods = new List<Food>();
        Tracks = new List<Track>();
        GameOver = false;
       // FindObjectOfType<SoundManager>().SourceMusic.volume = 0.5f;

        PrepareLevel();

        PrepareFoods();
        GenerateTracks();

        TxtLevelNumber.text = Level.ToString();
    }

    private void OnDestroy()
    {
     //   FindObjectOfType<SoundManager>().SourceMusic.volume = 1;
    }

    private void PrepareLevel()
    {
        LevelInfo = LevelSelector.LevelSelected;
        PanelResult.SetActive(false);

        if (LevelInfo == null)
        {
            SceneManager.LoadScene("Menu");
            return;
        }

        TotalTracks = LevelInfo.Tracks;
        Level = LevelInfo.Level;
        LimitTotalFoods = LevelInfo.TotalFoods;
        LimitFoodsGood = LevelInfo.TotalFoodsGood;
        SpeedGame = (float)LevelInfo.Speed / 10;
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

            gameObject.transform.localPosition = new Vector3(0, positionY, -0.1f);

            Tracks.Add(track);
        }
    }

    private void PrepareFoods()
    {
        FoodsToCreated = Enumerable.Range(0, LimitTotalFoods).Select(x => FoodType.Bad).ToList();

        var random = new System.Random();
        IList<int> indexGoodFood = Enumerable.Range(0, LimitTotalFoods).OrderBy(x => random.Next()).Take(LimitFoodsGood).ToList();

        for (int i = 0; i < indexGoodFood.Count; i++)
        {
            FoodsToCreated[indexGoodFood[i]] = FoodType.Good;
        }
    }

    private void ClearGrid()
    {
        foreach (Track track in Tracks)
        {
            Destroy(track.gameObject);
        }

        Tracks.Clear();
    }

    void Update()
    {
        if (!GameFinished)
        {
            DetectTapFood();

            switch (GameType)
            {
                case GameType.Limit:
                    UpdateGameModeLimit();
                    break;
            }
        }
    }

    private void UpdateGameModeLimit()
    {
        foreach (Track track in Tracks)
        {
            if (track.CanInsertFood && TotalFoodsCreated < LimitTotalFoods)
            {
                Food food = track.GetComponent<Track>().InsertFood(FoodsToCreated[TotalFoodsCreated]);

                if (food != null)
                {
                    Foods.Add(food);
                    TotalFoodsCreated++;
                }
            }
        }
    }

    private void DetectTapFood()
    {
        for (int i = 0; i < Input.touchCount; i++)
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
        if (GameFinished)
        {
            return;
        }

        Food food = objectFood.GetComponent<Food>();

        bool foodNotCollided = false;

        if (!food.CollidedWithCollider)
        {
            foodNotCollided = true;
        }
        //IMPEDIR DE CLICAR EM FOOD QND TERMINAR O JOGO
        if (food.Type == FoodType.Bad)
        {
            TotalBadFoodsClicked++;

            if (TotalBadFoodsClicked > ErrorsAllowed)
            {
                GameFinished = true;
                GameOver = true;
                PanelResult.GetComponent<PanelResult>().SetScore(Score.Bad, LevelInfo.Level);
            }
        }
        else
        {
            TotalGoodFoodsClicked++;
        }

        food.Track.DeleteFood(food);

        Destroy(objectFood);

        if (foodNotCollided)
        {
            food.Track.EnableInsertFood();
        }
    }

    public void VerifyFinished()
    {
        if (LimitTotalFoods == TotalFoodsCreated && Foods.Count == 0 && !GameOver)
        {
            GameFinished = true;
            CalculateScore();
        }
    }

    public void CalculateScore()
    {
        //Garantir que números impares, arredonde para cima. 
        //Ex: (9 / 2 ) + (9 % 2) = 4 + 1 = 5
        //Ex: (8 / 2 ) + (8 % 2) = 4 + 0 = 4
        int halfGood = (LimitFoodsGood / 2) + (LimitFoodsGood % 2);

        Score score = Score.Bad;

        if (TotalGoodFoodsClicked == LimitFoodsGood)
        {
            score = Score.Awesome;
        }
        else if (TotalGoodFoodsClicked >= halfGood)
        {
            float perc = ((float)TotalGoodFoodsClicked * 100f) / (float)LimitFoodsGood;

            if (perc >= 70)
            {
                score = Score.Good;
            }
            else
            {
                score = Score.Ok;
            }
        }

        GameFinished = true;
        PanelResult.GetComponent<PanelResult>().SetScore(score, LevelInfo.Level);
    }

    public void DeleteFood(Food food)
    {
        Foods.Remove(food);
    }
}
