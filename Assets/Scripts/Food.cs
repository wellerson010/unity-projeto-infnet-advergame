using Assets.Scripts.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {
    public float Speed;
    public FoodType Type;
    public List<Sprite> GoodFoods;
    public List<Sprite> BadFoods;
    public bool CollidedWithCollider = false;
    public Track Track;

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

    private void OnMouseDown()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().FoodClicked(gameObject);
    }
}
