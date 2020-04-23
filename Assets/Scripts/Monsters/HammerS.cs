using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerS : Monster
{
    Monster monster;

    public AudioClip AttackEffectClip;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetHammerS();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetHammerS()
    {
        monster.hp = 20;
        monster.moveSpd = 3;
        monster.stopDis = 1;
        monster.attackRange = 2;
        monster.attackDelay = 2;
        monster.attackDmg = 5;

        monster.currentState = MonsterState.Idle;
        monster.currentType = MonsterType.Short;
    }

    public void AttackSend()
    {
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            monster.target.GetComponent<PlayerController>().StartUnBeatTime();
            monster.target.GetComponent<PlayerController>().sendAttack((int)monster.attackDmg);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && monster.currentState != MonsterState.Die)
        {
            collision.gameObject.SetActive(false);
            monster.GetDmg(collision.GetComponent<PlayerBullet>().AttackPower);
        }
    }

    public void AttackEffect()
    {
        SoundManager.Instance.PlayMonsterEffect(AttackEffectClip);
    }
}
