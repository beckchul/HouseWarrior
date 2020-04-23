using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class YouDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TimeCheck");
    }
    IEnumerator TimeCheck()
    {
        int countTime = 0;

        while (countTime < 5)
        {
           
            yield return new WaitForSeconds(1.0f);
            countTime++;
        }

        SceneManager.LoadScene("Logo");
    
        yield return null;
    }

}
