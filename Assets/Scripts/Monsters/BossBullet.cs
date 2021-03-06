﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float moveSpd = 10;

    private float hamDmg;
    private Transform targetTrans;
    private Vector3 targetPos;
    private bool arrivePos = false;

    public void SetTarget(Transform target, float dmg)
    {
        hamDmg = dmg;
        targetTrans = target;
        targetPos = targetTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpd * Time.deltaTime);

        if (transform.position == targetPos && arrivePos != true)
        {
            arrivePos = true;
            Invoke("Disappear", 0.1f);
        }
    }

    void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().StartUnBeatTime();
            collision.gameObject.GetComponent<PlayerController>().sendAttack((int)hamDmg);
            Destroy(gameObject);
        }
    }
}
