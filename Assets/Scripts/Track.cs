using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Enum;

public class Track : MonoBehaviour {
    public float Speed;
    public bool CanInsertFood = true;
    public GameObject SpawnFood;
    public GameObject FoodCollisor;
    public GameObject Food;
    public IList<Food> Foods;

    private GameManager GameManager;
    private float TimeLastFoodInserted = 0;
    private Renderer Renderer;

    private void Awake()
    {
        GetComponent<Renderer>().sortingLayerName = "Game";
    }

    private void Start()
    {
        Renderer = GetComponent<Renderer>();
        Foods = new List<Food>();
        GameManager = FindObjectOfType<GameManager>();

      /*  float offsetInitial = Random.Range(0f, 1f);
        ChangeTextureOffset(offsetInitial);
        Debug.Log("Start " + offsetInitial); */
    } 

    private void Update()
    {
        ChangeTextureOffset();

        TimeLastFoodInserted += Time.deltaTime;

        if (TimeLastFoodInserted >= 1)
        {
            CanInsertFood = true;
        }
    }

    private void ChangeTextureOffset(float value = 0)
    {
        Vector2 pos = new Vector2(Speed * Time.deltaTime + value, 0);
        Renderer.material.mainTextureOffset += pos;
    }

    public Food InsertFood(FoodType type)
    {
        if (CanInsertFood)
        {
            TimeLastFoodInserted = 0;
            GameObject obj = Instantiate(Food, SpawnFood.transform.position, Quaternion.identity);
            Food food = obj.GetComponent<Food>();
            food.Speed = Speed;
            food.Track = this;
            food.Type = type;

            Foods.Add(food);
            CanInsertFood = false;

            return food;
        }

        return null;
    }

    public void EnableInsertFood()
    {
        for(int i=0; i < Foods.Count; i++)
        {
            if (!Foods[i].CollidedWithCollider)
            {
                return;
            }
        }

        CanInsertFood = true;
    }

    public void DeleteFood(Food food)
    {
        GameManager.DeleteFood(food);
        Foods.Remove(food);
        GameManager.VerifyFinished();
    }
}
