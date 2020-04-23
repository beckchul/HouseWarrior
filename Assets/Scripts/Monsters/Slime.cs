using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    Monster monster;

    private void Awake()
    {        
        monster = GetComponent<Monster>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetSlime();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void SetSlime()
    {
        monster.hp = 3;
        monster.moveSpd = 2;
        monster.stopDis = 3;
        monster.attackRange = 4;
        monster.attackDelay = 0.5f;
        monster.attackDmg = 2;

        monster.currentState = MonsterState.Idle;
        monster.currentType = MonsterType.Long;
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
        if (collision.CompareTag("Bullet") && monster.currentState != MonsterState.Die){
            collision.gameObject.SetActive(false);
            monster.GetDmg(collision.GetComponent<PlayerBullet>().AttackPower); // player dmg
        }
	}
}
