using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerL : Monster
{
    Monster monster;
    public GameObject ham;

    public AudioClip throwEffectClip;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetHammerL();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetHammerL()
    {
        monster.hp = 4;
        monster.moveSpd = 2;
        monster.stopDis = 5;
        monster.attackRange = 6;
        monster.attackDelay = 1;
        monster.attackDmg = 3;

        monster.currentState = MonsterState.Idle;
        monster.currentType = MonsterType.Long;
    }

    public void ThrowHam()
    {
        GameObject hamTmp = Instantiate<GameObject>(ham);
        hamTmp.transform.position = new Vector2(transform.position.x + 1, transform.position.y + 1);
        hamTmp.GetComponent<Ham>().SetTarget(monster.target, monster.attackDmg);

        SoundManager.Instance.PlayMonsterEffect(throwEffectClip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && monster.currentState != MonsterState.Die)
        {
            collision.gameObject.SetActive(false);
            monster.GetDmg(collision.GetComponent<PlayerBullet>().AttackPower); // player dmg
        }
    }

}
