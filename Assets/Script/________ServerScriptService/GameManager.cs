using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action WinGame;
    public Action ResetGame;

    public Action NextLever;
    public Action SelectLever;

    void Start()
    {
        WinGame+= WinGameFun;
    }


    public void WinGameFun()
    {
        Debug.Log("Đã WinGAme");
    }
}
