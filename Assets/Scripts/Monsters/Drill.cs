using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : Monster
{
    Monster monster;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetDrill();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

	private void SetDrill()
    {
        monster.hp = 10;
        monster.moveSpd = 3;
        monster.stopDis = 1;
        monster.attackRange = 2;
        monster.attackDelay = 1.9f;
        monster.attackDmg = 4;

        monster.currentState = MonsterState.Idle;
        monster.currentType = MonsterType.Short;
    }

    public void AttackSend()
    {
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
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
}
