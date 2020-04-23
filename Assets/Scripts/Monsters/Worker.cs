using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorkerAttackState{
    None, Long, Short
}

public class Worker : Monster
{
    static float atkSpdL = 3;
    static float atkDmgL = 5;
    static float atkRanL = 5;

    static float atkSpdS = 2;
    static float atkDmgS = 10;
    static float atkRanS = 2;

    Monster monster;
    WorkerAttackState curAtkState = WorkerAttackState.None;

    public GameObject bullet;

    public AudioClip bossThrowClip;
    public AudioClip bossAttackClip;

    private void Awake()
    {
        monster = GetComponent<Monster>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        SetWorker();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (monster.currentState == MonsterState.Attack){
            float distance = Vector2.Distance(transform.position, monster.target.position);

            if (distance <= atkRanS && curAtkState != WorkerAttackState.Short){
                curAtkState = WorkerAttackState.Short;
                monster.attackDmg = atkDmgS;

                StartCoroutine("AttackCo");
            }
            else if (distance <= atkRanL && distance > atkRanS && curAtkState != WorkerAttackState.Long){
                curAtkState = WorkerAttackState.Long;
                monster.attackDmg = atkDmgL;

                StartCoroutine("AttackCo");
            }
        }else{
            curAtkState = WorkerAttackState.None;
            StopCoroutine("AttackCo");
        }
    }

    IEnumerator AttackCo()
    {
        while (currentState == MonsterState.Attack)
        {
            if (curAtkState == WorkerAttackState.Short){                
                animator.SetTrigger("AttackS");
                yield return new WaitForSeconds(atkSpdS);
            }
            else if (curAtkState == WorkerAttackState.Long){
                animator.SetTrigger("AttackL");
                yield return new WaitForSeconds(atkSpdL);
            }
        }
    }

    private void SetWorker()
    {
        monster.hp = 100;
        monster.moveSpd = 3;
        monster.stopDis = 1;
        monster.attackRange = 5;
        monster.attackDelay = 3;
        monster.attackDmg = 5;

        monster.currentState = MonsterState.Idle;
        monster.currentType = MonsterType.Boss;
    }

    public void AttackSend()
    {
        SoundManager.Instance.PlayEffect(bossAttackClip);
        monster.attackDmg = atkDmgS;
        if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            monster.target.GetComponent<PlayerController>().sendAttack((int)monster.attackDmg);
        }
    }

    public void ShotBullet()
    {
        monster.attackDmg = atkDmgL;
        GameObject bulletTmp = Instantiate<GameObject>(bullet);
        bulletTmp.transform.position = new Vector2(transform.position.x + 1, transform.position.y + 1);
        bulletTmp.GetComponent<BossBullet>().SetTarget(monster.target, monster.attackDmg);

        SoundManager.Instance.PlayMonsterEffect(bossThrowClip);
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
