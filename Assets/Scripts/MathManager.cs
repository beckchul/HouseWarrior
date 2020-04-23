using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathManager : MonoBehaviour
{
    // Start is called before the first frame update

    private static MathManager instance;
    private static GameObject container;
    public static MathManager GetInstance()
    {
        if (!instance)
        {
            container = new GameObject();
            container.name = "MathManager";
            instance = container.AddComponent(typeof(MathManager)) as MathManager;
            DontDestroyOnLoad(container);
        }
        return instance;
    }

    public float GetTargetAngle(Vector3 Dest, Vector3 Sour)
    {
        Vector3 dir = Dest - Sour;
        return Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
    }
}
