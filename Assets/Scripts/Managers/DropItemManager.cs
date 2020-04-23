using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoxItemType
{
    none, weapon, buff, defense
}

public class DropItemManager : MonoBehaviour
{
    public BoxItemType itemType;
    public Sprite weaponSprite;
    public Sprite buffSprite;
    public Sprite defenseSprite;
    public Sprite openSprite;

    public AudioClip boxOpenClip;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        switch (itemType)
        {
            case BoxItemType.weapon:
                InitWeapon();
                break;
            case BoxItemType.buff:
                InitBuff();
                break;
            case BoxItemType.defense:
                InitDefense();
                break;
            case BoxItemType.none:
                break;
        }
    }

    void InitWeapon()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        this.gameObject.tag = "WeaponBox";
    }

    void InitBuff()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = buffSprite;
        this.gameObject.tag = "BuffBox";
    }

    void InitDefense()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defenseSprite;
        this.gameObject.tag = "DefenseBox";
    }

    public void OpenBox()
    {
        SoundManager.Instance.PlayEffect(boxOpenClip);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = openSprite;
        this.gameObject.tag = "OpenBox";
        Invoke("remove", 0.1f);
    }
    void remove()
    {
        Destroy(gameObject);
    }
}
