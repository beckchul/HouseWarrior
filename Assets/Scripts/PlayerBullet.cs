using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 vecDir;
    float BulletAngle;
    public float BulletSpeed = 1000.0f;
    Rigidbody2D rb;
    float roopTime = 2.0f;
    float TimeCheck = 0.0f;
    public bool FireCheck = false;

    public int AttackPower;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
       
    // Update is called once per frame
    void Update()
    {
        if (FireCheck)
        {
            Vector3 mos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            mos.z = 0.0f;
            BulletAngle = MathManager.GetInstance().GetTargetAngle(mos, transform.position);
            Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, -BulletAngle);
            transform.rotation = rotation;
            vecDir = mos - transform.position;

            rb.AddForce(vecDir.normalized * BulletSpeed);
            FireCheck = false;
        }
       

        TimeCheck += Time.deltaTime;
        if (roopTime <= TimeCheck)
        {
            TimeCheck = 0.0f;
            gameObject.SetActive(false);
        }
    }
}
