using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    public List<Sprite> GoodFoods;
    public List<Sprite> BadFoods;

    [HideInInspector]
    public FoodType Type;

    [HideInInspector]
    public bool CollidedWithCollider = false;

    [HideInInspector]
    public Track Track;

    [HideInInspector]
    public float Speed;

    private SpriteRenderer Sprite;

    void Start () {
        Sprite = GetComponent<SpriteRenderer>();

        ChangeSprite(Type);
	}

    private void ChangeSprite(FoodType type)
    {
        IList<Sprite> sprites = (type == FoodType.Bad) ? BadFoods : GoodFoods;
        Sprite.sprite = sprites[Random.Range(0, sprites.Count)];
    }
	
	void Update () {
        Vector3 position = Vector3.left * (Speed * 3.7f) * Time.deltaTime;
        transform.Translate(position);
	}

    private void OnBecameInvisible()
    {
        Track.DeleteFood(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollidedWithCollider = true;
        Track.EnableInsertFood();
    }

    #if UNITY_EDITOR
    private void OnMouseDown()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().FoodClicked(gameObject);
    }
    #endif
}
