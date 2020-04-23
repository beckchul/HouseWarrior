using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    private static GameObject container;

    public GameObject PlayerBullet;
    public GameObject PlayerObject;
    public List<GameObject> PoolBlletlist;
    public int PoolObjectCount = 48;
    void Start()
    {
        PoolBlletlist = new List<GameObject>();

        for (int i = 0; i < PoolObjectCount; ++i)
        {
            GameObject ObjBullet = Instantiate(PlayerBullet);
            ObjBullet.transform.parent = PlayerObject.transform;

            ObjBullet.SetActive(false);
            PoolBlletlist.Add(ObjBullet);
        }
    }

    public static PoolManager GetInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "PoolManager";
            instance = container.AddComponent(typeof(PoolManager)) as PoolManager;
            instance.PlayerBullet = Resources.Load("PlayerBullet") as GameObject;
            instance.PlayerObject = GameObject.Find("PlayObject").gameObject;
        }
        return instance;
    }

    public GameObject GetPoolObjectPlayerBullet()
    {
        for(int i = 0; i < PoolBlletlist.Count; ++i)
        {
            if (!PoolBlletlist[i].activeInHierarchy)
                return PoolBlletlist[i];
        }

        return null;
    }
}
