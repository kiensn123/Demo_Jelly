using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action WinGame;
    public Action ResetGame;

    public Action NextLever;
    public Action SelectLever;


    public List<LeverData> leverDatas;

    public Transform Spawn;



    public GameObject LeverInGame;
    public TextMeshProUGUI LeverUI;

    public int LeverIndex;
    void Start()
    {
        WinGame+= WinGameFun;
        SetLever(LeverIndex);
    }


    public void WinGameFun()
    {
        Debug.Log("Đã WinGAme");
    }




    public void SetLever(int lever)
    {
        if(LeverInGame){Destroy(LeverInGame);}
        LeverInGame = Instantiate(leverDatas[lever].Map,Spawn);
        LeverInGame.transform.localPosition = Vector3.zero;
     
        LeverIndex = lever;
           LeverUI.text = "LEVER : "+ (LeverIndex +1);
    }

    public void ResetLever()
    {
        SetLever(LeverIndex);
    }

}
