using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // Start is called before the first frame update

    public float RotationAngle = 0.0f;
    // Update is called once per frame
    void Update()
    {
        RotationGun();
    }

    void RotationGun()
    {
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, -RotationAngle);
        transform.rotation = rotation;
    }
}
