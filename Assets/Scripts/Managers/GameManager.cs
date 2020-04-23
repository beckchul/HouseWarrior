using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct Wave
{
    public int hammerLNum;
    public int hammerSNum;
    public int workerNum;

    public Wave(int a, int b, int c){
        hammerLNum = a;
        hammerSNum = b;
        workerNum = c;
    }
};

public enum CurrentWave
{
    p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
                if (_instance == null)
                {
                    Debug.LogError("Error GameManager");
                }
            }
            return _instance;
        }
    }

    private Wave[] waves = new Wave[]
    {
        new Wave (2,2,0),
        new Wave (2,4,0),
        new Wave (2,6,0),
        new Wave (3,9,0),
        new Wave (5,10,0),
        new Wave (2,6,1),
        new Wave (3,6,1),
        new Wave (3,6,2),
        new Wave (3,9,2),
        new Wave (5,10,3)
    };

    public bool readyWave = false;
    public bool startWave = false;
    public bool itemTime = false;
    public bool uiTime = false;

    public Text uiText;

    public GameObject[] monsterPrefabs;

    public GameObject boxItem;

    private CurrentWave curWave = CurrentWave.p0;
    private GameObject[] spawnPoint;
    private List<GameObject> monsters = new List<GameObject>();

    private GameObject player;

	private void Awake()
	{
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GameObject.FindGameObjectsWithTag("SpawnPoint");
	}

    // Update is called once per frame
    void Update()
    {
        if (readyWave && itemTime != true){
            readyWave = false;

            switch(curWave){
                case CurrentWave.p1:
                    StartWave(waves[0]);
                    break;
                case CurrentWave.p2:
                    StartWave(waves[1]);
                    break;
                case CurrentWave.p3:
                    StartWave(waves[2]);
                    break;
                case CurrentWave.p4:
                    StartWave(waves[3]);
                    break;
                case CurrentWave.p5:
                    StartWave(waves[4]);
                    break;
                case CurrentWave.p6:
                    StartWave(waves[5]);
                    break;
                case CurrentWave.p7:
                    StartWave(waves[6]);
                    break;
                case CurrentWave.p8:
                    StartWave(waves[7]);
                    break;
                case CurrentWave.p9:
                    StartWave(waves[8]);
                    break;
                case CurrentWave.p10:
                    StartWave(waves[9]);
                    break;
            }

            startWave = false;
        }

        if (monsters.Count == 0 && startWave == false && readyWave == false) {
            startWave = true;

            switch (curWave)
            {
                case CurrentWave.p0:
                    curWave = CurrentWave.p1;
                    WaveUIText("Wave 1");
                    break;
                case CurrentWave.p1:
                    curWave = CurrentWave.p2;
                    WaveUIText("Wave 2");
                    DropBox();
                    break;
                case CurrentWave.p2:
                    curWave = CurrentWave.p3;
                    WaveUIText("Wave 3");
                    DropBox();
                    break;
                case CurrentWave.p3:
                    curWave = CurrentWave.p4;
                    WaveUIText("Wave 4");
                    DropBox();
                    break;
                case CurrentWave.p4:
                    curWave = CurrentWave.p5;
                    WaveUIText("Wave 5");
                    DropBox();
                    break;
                case CurrentWave.p5:
                    curWave = CurrentWave.p6;
                    WaveUIText("Wave 6");
                    DropBox();
                    break;
                case CurrentWave.p6:
                    curWave = CurrentWave.p7;
                    WaveUIText("Wave 7");
                    break;
                case CurrentWave.p7:
                    curWave = CurrentWave.p8;
                    WaveUIText("Wave 8");
                    DropBox();
                    break;
                case CurrentWave.p8:
                    curWave = CurrentWave.p9;
                    WaveUIText("Wave 9");
                    DropBox();
                    break;
                case CurrentWave.p9:
                    curWave = CurrentWave.p10;
                    WaveUIText("Wave 10");
                    DropBox();
                    break;
                case CurrentWave.p10:
                    break;
            }

            readyWave = true;
        }
    }

    void StartWave(Wave wave){
        int hammerLNum = wave.hammerLNum;
        int hammerSNum = wave.hammerSNum;
        int workerNum = wave.workerNum;

        int totalNum = hammerLNum + hammerSNum + workerNum;
        List<int> inxes = new List<int>();

        while(totalNum != inxes.Count)
        {
            int tmpInx = Random.Range(0, spawnPoint.Length);
            if (!inxes.Contains(tmpInx))
            {
                inxes.Add(tmpInx);
            }
        }

        List<int>.Enumerator enumerator = inxes.GetEnumerator();
        enumerator.MoveNext();

        for (int i = 0; i < hammerLNum; i++)
        {
            GameObject monTmp = Instantiate<GameObject>(monsterPrefabs[0]);
            monTmp.transform.position = spawnPoint[enumerator.Current].transform.position;
            monsters.Add(monTmp);
            enumerator.MoveNext();
        }
        for (int i = 0; i < hammerSNum; i++)
        {
            GameObject monTmp = Instantiate<GameObject>(monsterPrefabs[1]);
            monTmp.transform.position = spawnPoint[enumerator.Current].transform.position;
            monsters.Add(monTmp);
            enumerator.MoveNext();
        }
        for (int i = 0; i < workerNum; i++)
        {
            GameObject monTmp = Instantiate<GameObject>(monsterPrefabs[2]);
            monTmp.transform.position = spawnPoint[enumerator.Current].transform.position;
            monsters.Add(monTmp);
            enumerator.MoveNext();
        }
    }

    void DropBox()
    {
        int percent = Random.Range(1, 10);
        if (percent >= 1 && percent <= 3)
        {
            GameObject boxTmp = Instantiate<GameObject>(boxItem);
            boxTmp.GetComponent<DropItemManager>().itemType = BoxItemType.weapon;
            boxTmp.transform.position = player.transform.position + new Vector3(5, 0, 0);
        }
        else if (percent > 3 && percent <= 6)
        {
            GameObject boxTmp = Instantiate<GameObject>(boxItem);
            boxTmp.GetComponent<DropItemManager>().itemType = BoxItemType.buff;
            boxTmp.transform.position = player.transform.position + new Vector3(5, 0, 0);
        }
        else if (percent > 6 && percent <= 10)
        {
            GameObject boxTmp = Instantiate<GameObject>(boxItem);
            boxTmp.GetComponent<DropItemManager>().itemType = BoxItemType.defense;
            boxTmp.transform.position = player.transform.position + new Vector3(5, 0, 0);
        }

    }

    public void RemoveMonstersList(GameObject monster)
    {
        monsters.Remove(monster);
    }

    void WaveUIText(string str)
    {
        uiText.text = str;
        Invoke("RemoveText", 2f);
    }
    void RemoveText()
    {
        uiText.text = "";
    }
}
