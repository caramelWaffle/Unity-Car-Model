using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAction : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Button1Clicked()
    {
        Scenes.Load("CarScene", "task", "1");
    }

    public void Button2Clicked()
    {
        Scenes.Load("CarScene", "task", "2");
    }

    public void ButtonSwapClicked()
    {
        //Debug.Log("is swap");
        Scenes.Load("CarScene", "task", "3");
    }

    public void doExitGame()
    {
        Application.Quit();
    }
}
