using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour {
    public static Action OnGameStart = delegate { };
    public static Action OnGameReset = delegate { };
    public static Action OnNextLevel = delegate { };    
    public static event Action<int> OnLevelSetup;
    public static event Action OnGameFinish;
    public static event Action OnGameOver;
    public static void StartGame()
    {
        OnGameStart();
    }
    public static void LevelReset()
    {
        OnGameReset();
    }
    public static void GameOver()
    {
        OnGameOver();
    }
    public static void LevelSetup(int _levelnumber){
        OnLevelSetup(_levelnumber);
    }
    
    public static void GameFinish(){
        OnGameFinish();
    }
    public static void Nextlevel(){
        OnNextLevel();
    }
  
}