using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    none, bullet, hp
}

public class ItemManager : MonoBehaviour
{

    public ItemType itemType;
    public Sprite bulletSprite;
    public Sprite hpSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        switch (itemType)
        {
            case ItemType.bullet:
                InitBullet();
                break;
            case ItemType.hp:
                InitHp();
                break;
            case ItemType.none:
                break;
        }
    }

    void Update()
    {
        
    }

    void InitBullet()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bulletSprite;
        this.gameObject.tag = "BulletItem";
    }
    void InitHp()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = hpSprite;
        this.gameObject.tag = "HpItem";
    }

    void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("Disappear", 0.1f);
        }
    }
}

