using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float hp;
    public float rotSpd = 10;
    public float moveSpd;
    public float stopDis;
    public float attackRange;
    public float attackDelay;
    public float attackDmg;
    public MonsterState currentState;
    public MonsterType currentType;

    public Transform target;

    public Rigidbody2D rigi;

    public Animator animator;

    public GameObject item;

    private SpriteRenderer render;

    private bool facingRight = false;
    private bool facingLeft = false;

    protected virtual void Start(){
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update(){
        if (currentState == MonsterState.Die) return;

        if (hp <= 0 && currentState != MonsterState.Die){
            currentState = MonsterState.Die;
            Die();
        }
        if (Vector2.Distance(transform.position, target.position) > attackRange && currentState != MonsterState.Move)
        {
            currentState = MonsterState.Idle;
        }
        if (currentType == MonsterType.Boss)
        {
            if (Vector2.Distance(transform.position, target.position) > stopDis)
            {
                if (currentState != MonsterState.Move)
                {
                    currentState = MonsterState.Move;
                }
                Vector3 vectorToTarget = target.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion quar = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, quar, Time.deltaTime * rotSpd);

                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpd * Time.deltaTime);
                if (currentState != MonsterState.Attack)
                {
                    animator.SetBool("Move", true);
                }
            }
            else
            {
                animator.SetBool("Move", false);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > stopDis && currentState != MonsterState.Attack)
            {
                if (currentState != MonsterState.Move)
                {
                    currentState = MonsterState.Move;
                }
                Vector3 vectorToTarget = target.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion quar = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, quar, Time.deltaTime * rotSpd);

                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpd * Time.deltaTime);
                animator.SetBool("Move", true);

            }
            else
            {
                animator.SetBool("Move", false);
            }
        }




        if (transform.position.x > target.position.x && facingLeft == false)
        {
            facingLeft = true;
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
            render.flipY = true;
        }
        else if (transform.position.x < target.position.x && facingRight == false)
        {
            facingLeft = false;
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
            render.flipY = false;
        }

        if (Vector2.Distance(transform.position, target.position) <= attackRange){
            if (currentState != MonsterState.Attack)
            {
                currentState = MonsterState.Attack;

                switch (currentType)
                {
                    case MonsterType.Long:
                        LongAttack();
                        break;
                    case MonsterType.Short:
                        ShortAttack();
                        break;
                    case MonsterType.Boss:                        
                        break;
                }
            }
        }
    }
    protected virtual void ShortAttack(){
        StartCoroutine("AttackCo");
    }
    protected virtual void LongAttack(){        
        StartCoroutine("AttackCo");
    }

    IEnumerator AttackCo(){
        while(currentState == MonsterState.Attack){
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDelay);
        }
        if(currentState != MonsterState.Attack){
            StopCoroutine("AttackCo");
        }
    }

    public void GetDmg(float dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            Die();
        }

        currentState = MonsterState.Hit;

        //StartCoroutine(KnockBack(0.02f, 350, transform.position));

        StartCoroutine("UnBeatTime");
    }

    IEnumerator UnBeatTime()
    {
        int countTime = 0;

        while(countTime < 6)
        {
            if (countTime % 2 == 0)
                render.color = new Color32(255, 100, 100, 90);
            else
                render.color = new Color32(255, 100, 100, 180);

            yield return new WaitForSeconds(0.2f);

            countTime++;
        }
        render.color = new Color32(255, 255, 255, 255);


        yield return null;
    }

    //private IEnumerator KnockBack(float knockDur, float knockbackPwr, Vector3 knockbackDir)
    //{
    //    float timer = 0;

    //    while (knockDur > timer)
    //    {
    //        timer += Time.deltaTime;
    //        Vector3 pushDir = transform.position - target.transform.position;
    //        rigi.AddForce(pushDir.normalized * 10);
    //    }

    //    yield return 0;
    //}

    protected virtual void Die(){
        //animator.SetBool("Die", true);
        DropItem();
        GameManager.Instance.RemoveMonstersList(gameObject);
        Destroy(gameObject);
    }

    void DropItem()
    {
        int percent = Random.Range(1, 10);
        if (percent >= 1 && percent <= 6){
            //탄약
            GameObject itemTmp = Instantiate<GameObject>(item);
            itemTmp.GetComponent<ItemManager>().itemType = ItemType.bullet;
            itemTmp.transform.position = transform.position;
        }
        else if (percent > 6 && percent <= 10) {
            //체력
            GameObject itemTmp = Instantiate<GameObject>(item);
            itemTmp.GetComponent<ItemManager>().itemType = ItemType.hp;
            itemTmp.transform.position = transform.position;
        }
    }


}

public enum MonsterState
{
    Idle, Move, Attack, Die, Hit
}
public enum MonsterType
{
    Long, Short, Boss
}