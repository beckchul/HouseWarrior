using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    enum eWeaponType { WEAPON_PISTOL, WEAPON_SUBMACHINE_GUN, WEAPON_MACHINE_GUN, WEAPON_SINPER_RIFLE, WEAPON_HEAVY_MACHINE_GUN, WEAPON_END };
    enum eHouseType { HOUSE_BOX, HOUSE_WOOD, HOUSE_BLOCK, HOUSE_APT, HOUSE_END };

    public int PlayerHp;
    public int PlayerHpMax;
    public int PlayerAtt;
    public int PlusAtt = 0;
    public float PlayerMoveSpeed;
    public float PlusMoveSpeed = 0.0f;

    private Vector2 vecMove;
    private float PlayerAngle;

    Rigidbody2D rb;
    public int MaxWeaponCount = 4;          // 최대 총 오브젝트 수
    int WeaponCount = 1;                    // 총 오브젝트 수
    public GameObject Bullet;               // Bullet
    eWeaponType PlayerWeaponType = eWeaponType.WEAPON_PISTOL;
    eWeaponType PlayerCurWeaponType = eWeaponType.WEAPON_END;

    eHouseType PlayerHouseType = eHouseType.HOUSE_BOX;
    eHouseType PlayerCurHouseType = eHouseType.HOUSE_END;


    float AttackDelay = 1.0f;           // 공격 딜레이
    float AttackDelayTime = 0.0f;       // 공격 딜레이 시간계산
    float Reload = 2.1f;                // 재장전
    float ReloadTime = 0.0f;            // 재장전 시간계산
    public int BulletCount = 7;         // 현재총알
    public int MaxBulletCount = 7;      // 최대총알
    public int SubBulletCount = 32;     // 예비총알
    bool FirstShot = true;              // 처음발사인지
    bool IsReload = false;              // 재장전중인지
    public Sprite[] WeaponSprite;       // 무기 이미지
    public Sprite[] HouseSprite;        // 집 이미지
    SpriteRenderer render;

    Slider HpBar;
    GameObject RoadingBar;
    Text BulletCountText;
    Text HpText;

    public AudioClip[] ShotSounbd;
    public AudioClip[] ReloadSound;
    public AudioClip[] HouseSound;
    public AudioClip ItemSound;

    public void sendAttack(int Damage)
    {
        PlayerHp -= Damage;
        HpBar.value = (float)PlayerHp / (float)PlayerHpMax;

        switch(PlayerHouseType)
        {
            case eHouseType.HOUSE_BOX:
                SoundManager.Instance.PlayEffect(HouseSound[0]);
                break;
            case eHouseType.HOUSE_WOOD:
                SoundManager.Instance.PlayEffect(HouseSound[1]);
                break;
            case eHouseType.HOUSE_BLOCK:
                SoundManager.Instance.PlayEffect(HouseSound[2]);
                break;
            case eHouseType.HOUSE_APT:
                SoundManager.Instance.PlayEffect(HouseSound[3]);
                break;
        }


        if (0 >= PlayerHp)
        {
            SceneManager.LoadScene("YouDie");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        PoolManager.GetInstance();
        HpBar = GameObject.Find("HpBar").GetComponent<Slider>();
        RoadingBar = GameObject.Find("RoadingBar");
        RoadingBar.SetActive(false);
        BulletCountText = GameObject.Find("BulletCount").GetComponent<Text>();
        HpText = GameObject.Find("HpText").GetComponent<Text>();
        HpBar.value = 1.0f;
        ChangeWeapon(0);
        ChangeHouse(0);
    }

    // Update is called once per frame
    void Update()
    {
        KeyCheck();
        MouseCheck();
        WeaponReload();

        BulletCountText.text = "Bullet : " + BulletCount.ToString() + " / " + SubBulletCount.ToString();
        HpText.text = PlayerHp.ToString() + " / " + PlayerHpMax.ToString();
    }

    void KeyCheck()
    {
        vecMove = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.W))
            ++vecMove.y;

        if (Input.GetKey(KeyCode.A))
            --vecMove.x;

        if (Input.GetKey(KeyCode.S))
            --vecMove.y;

        if (Input.GetKey(KeyCode.D))
            ++vecMove.x;

        
    }

    private void FixedUpdate()
    {
        rb.velocity = vecMove.normalized;
        rb.velocity = rb.velocity * (PlayerMoveSpeed + PlusMoveSpeed) * Time.fixedDeltaTime;

        if (transform.position.x > 30)
            transform.position = new Vector3(30.0f, transform.position.y, 0.0f);
        if (transform.position.x < -30)
            transform.position = new Vector3(-30.0f, transform.position.y, 0.0f);
        if (transform.position.y > 20)
            transform.position = new Vector3(transform.position.x, 20.0f, 0.0f);
        if (transform.position.y < -20)
            transform.position = new Vector3(transform.position.x, -20.0f, 0.0f);
    }

    void MouseCheck()
    {
        Vector3 mos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mos.z = 0.0f;
        PlayerAngle = MathManager.GetInstance().GetTargetAngle(mos, transform.position);
        LookRotation(PlayerAngle);  // 좌 우 보는방향 스케일 조정
        SettingWeaponAngle();       // 무기 각도 회전

        if (Input.GetMouseButton(1) && !IsReload && 0 != SubBulletCount)
        {
            IsReload = true;
            RoadingBar.SetActive(true);

            switch (PlayerWeaponType)   // 장전사운드넣는곳
            {
                case eWeaponType.WEAPON_PISTOL:
                    SoundManager.Instance.PlayEffect(ReloadSound[0]);
                    break;
                case eWeaponType.WEAPON_SUBMACHINE_GUN:
                    SoundManager.Instance.PlayEffect(ReloadSound[1]);
                    break;
                case eWeaponType.WEAPON_MACHINE_GUN:
                    SoundManager.Instance.PlayEffect(ReloadSound[2]);
                    break;
                case eWeaponType.WEAPON_SINPER_RIFLE:
                    SoundManager.Instance.PlayEffect(ReloadSound[3]);
                    break;
                case eWeaponType.WEAPON_HEAVY_MACHINE_GUN:
                    SoundManager.Instance.PlayEffect(ReloadSound[4]);
                    break;
            }
        }

        if (Input.GetMouseButton(0) && 0 < BulletCount && !IsReload)
        {
            if (FirstShot)
            {
                --BulletCount;
                CreateBullet();
                FirstShot = false;
                AttackDelayTime = 0.0f;

               
            }
            AttackDelayTime += Time.deltaTime;
            if (AttackDelay <= AttackDelayTime)
            {
                --BulletCount;
                AttackDelayTime = 0.0f;
                CreateBullet();

              
            }
        }
        else
        {
            AttackDelayTime += Time.deltaTime;
            if (AttackDelay <= AttackDelayTime)
                FirstShot = true;
        }
    }

    void LookRotation(float Angle)
    {
        if (0.0f < Angle)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
    }

    void SettingWeaponAngle()
    {
        for (int i = 1; i <= WeaponCount; ++i)
        {
            string weaponName = "PlayerWeapon" + i.ToString();
            transform.Find(weaponName).GetComponent<PlayerWeapon>().RotationAngle = PlayerAngle;
        }
    }

    void CreateBullet()
    {
        switch (PlayerWeaponType)   // 발사사운드넣는곳
        {
            case eWeaponType.WEAPON_PISTOL:
                SoundManager.Instance.PlayEffect(ShotSounbd[0]);
                break;
            case eWeaponType.WEAPON_SUBMACHINE_GUN:
                SoundManager.Instance.PlayEffect(ShotSounbd[1]);
                break;
            case eWeaponType.WEAPON_MACHINE_GUN:
                SoundManager.Instance.PlayEffect(ShotSounbd[2]);
                break;
            case eWeaponType.WEAPON_SINPER_RIFLE:
                SoundManager.Instance.PlayEffect(ShotSounbd[3]);
                break;
            case eWeaponType.WEAPON_HEAVY_MACHINE_GUN:
                SoundManager.Instance.PlayEffect(ShotSounbd[4]);
                break;
        }

        for (int i = 1; i <= WeaponCount; ++i)
        {
            string weaponName = "PlayerWeapon" + i.ToString();
           

            GameObject Bullet = PoolManager.GetInstance().GetPoolObjectPlayerBullet();
            if (null == Bullet)
                return;
            Bullet.SetActive(true);
            Bullet.GetComponent<PlayerBullet>().FireCheck = true;
            Bullet.GetComponent<PlayerBullet>().AttackPower = PlayerAtt + PlusAtt;
            Bullet.transform.position = transform.Find(weaponName).GetComponent<PlayerWeapon>().transform.position;
        }
    }

    void WeaponReload()
    {
        if (!IsReload)
            return;

        ReloadTime += Time.deltaTime;
        RoadingBar.GetComponent<Slider>().value = ReloadTime / Reload;
        if (Reload <= ReloadTime)
        {
            ReloadTime = 0.0f;
            IsReload = false;
            RoadingBar.SetActive(false);
            if (SubBulletCount >= MaxBulletCount)
            {
                SubBulletCount -= MaxBulletCount;
                BulletCount = MaxBulletCount;
            }
            else
            {
                BulletCount = SubBulletCount;
                SubBulletCount = 0;
            }
        }
    }

    public void ChangeWeapon(int Type)
    {
        PlayerWeaponType = (eWeaponType)Type;
        if (PlayerCurWeaponType != PlayerWeaponType)
        {
            switch (PlayerWeaponType)
            {
                case eWeaponType.WEAPON_PISTOL:
                    AttackDelay = 1.0f;
                    Reload = 2.1f;
                    BulletCount = 7;
                    MaxBulletCount = 7;
                    PlayerAtt = 2;
                    SubBulletCount = 56;
                    MaxWeaponCount = 4;
                    WeaponCount = 1;
                    break;
                case eWeaponType.WEAPON_SUBMACHINE_GUN:
                    AttackDelay = 0.2f;
                    Reload = 3.1f;
                    BulletCount = 20;
                    MaxBulletCount = 20;
                    PlayerAtt = 2;
                    SubBulletCount = 80;
                    MaxWeaponCount = 3;
                    WeaponCount = 1;
                    break;
                case eWeaponType.WEAPON_MACHINE_GUN:
                    AttackDelay = 0.15f;
                    Reload = 2.9f;
                    BulletCount = 30;
                    MaxBulletCount = 30;
                    PlayerAtt = 3;
                    SubBulletCount = 90;
                    MaxWeaponCount = 2;
                    WeaponCount = 1;
                    break;
                case eWeaponType.WEAPON_SINPER_RIFLE:
                    AttackDelay = 2.0f;
                    Reload = 4.6f;
                    BulletCount = 5;
                    MaxBulletCount = 5;
                    PlayerAtt = 10;
                    SubBulletCount = 15;
                    MaxWeaponCount = 1;
                    WeaponCount = 1;
                    break;
                case eWeaponType.WEAPON_HEAVY_MACHINE_GUN:
                    AttackDelay = 0.07f;
                    Reload = 8.2f;
                    BulletCount = 100;
                    MaxBulletCount = 100;
                    PlayerAtt = 1;
                    SubBulletCount = 100;
                    MaxWeaponCount = 1;
                    WeaponCount = 1;
                    break;
            }
            ActiveWeapon();
            PlayerCurWeaponType = PlayerWeaponType;
        }
    }

    void ActiveWeapon()
    {
        for (int i = 1; i <= WeaponCount; ++i)
        {
            string weaponName = "PlayerWeapon" + i.ToString();
            transform.Find(weaponName).gameObject.SetActive(true);
            transform.Find(weaponName).GetComponent<SpriteRenderer>().sprite = WeaponSprite[(int)PlayerWeaponType];
        }

        for (int i = WeaponCount + 1; i <= 4; ++i)
        {
            string weaponName = "PlayerWeapon" + i.ToString();
            transform.Find(weaponName).gameObject.SetActive(false);
        }
    }

    void ChangeHouse(int Type)
    {
        PlayerHouseType = (eHouseType)Type;
        if (PlayerCurHouseType != PlayerHouseType)
        {
            switch (PlayerHouseType)
            {
                case eHouseType.HOUSE_BOX:
                    PlayerHp = 10;
                    PlayerHpMax = 10;
                    break;
                case eHouseType.HOUSE_WOOD:
                    PlayerHp = 50;
                    PlayerHpMax = 50;
                    break;
                case eHouseType.HOUSE_BLOCK:
                    PlayerHp = 100;
                    PlayerHpMax = 100;
                    break;
                case eHouseType.HOUSE_APT:
                    PlayerHp = 150;
                    PlayerHpMax = 150;
                    break;
            }
            GetComponent<SpriteRenderer>().sprite = HouseSprite[(int)PlayerHouseType];
            PlayerCurHouseType = PlayerHouseType;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletItem"))
        {
            SubBulletCount += 10;
            Destroy(collision.gameObject);
            SoundManager.Instance.PlayEffect(ItemSound);
        }
        if (collision.CompareTag("HpItem"))
        {
            PlayerHp += 5;
            if (PlayerHp > PlayerHpMax)
                PlayerHp = PlayerHpMax;

            HpBar.value = (float)PlayerHp / (float)PlayerHpMax;
            Destroy(collision.gameObject);
            SoundManager.Instance.PlayEffect(ItemSound);
        }
        if (collision.CompareTag("WeaponBox"))
        {
            int random = (int)Random.Range(0.0f, 4.0f);

            if (random == (int)PlayerWeaponType)
            {
                if (MaxWeaponCount > WeaponCount)
                {
                    WeaponCount++;
                    ActiveWeapon();
                }
            }
            else
                ChangeWeapon(random);

            collision.GetComponent<DropItemManager>().OpenBox();
        }
        if (collision.CompareTag("BuffBox"))
        {
            PlusMoveSpeed += 30.0f;
            collision.GetComponent<DropItemManager>().OpenBox();
        }
        if (collision.CompareTag("DefenseBox"))
        {
            int iDef = (int)PlayerHouseType;
            if(iDef < (int)eHouseType.HOUSE_APT)
                ChangeHouse(++iDef);
            collision.GetComponent<DropItemManager>().OpenBox();
            HpBar.value = (float)PlayerHp / (float)PlayerHpMax;
        }
    }

    public void StartUnBeatTime()
    {
        StartCoroutine("UnBeatTime");
    }

    IEnumerator UnBeatTime()
    {
        int countTime = 0;

        while (countTime < 6)
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
}

