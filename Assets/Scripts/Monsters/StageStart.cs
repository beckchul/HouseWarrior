using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageStart : MonoBehaviour
{
   public  void ChangeStage()
    {
        SceneManager.LoadScene("Stage");
    }
    public void ExitScene()
    {
        Application.Quit();
    }
}
