using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : GenericS<GameManager>
{
    public LevelCreators Alllevels;
    public CinemachineVirtualCamera C_camera;
    public GameObject Player,Aiplayer,C_Player,C_lvl;
    public List<GameObject> C_Aiplayer;
    [HideInInspector]
    public LevelGenerator levelGenerator;
    public float AiplayerCount;
    int _levelnumber;
    public int LevelNumber{
        get{
            return _levelnumber;
        }
        set{
            _levelnumber=value;
            DataManager.Instance.data.Current_Level = _levelnumber;
            GamePlayScreen.Instance.Lvl_txt.text=(_levelnumber+1).ToString();
            GamePlayScreen.Instance.NextLvl_txt.text=(_levelnumber+2).ToString();
        }
    }

    void OnEnable()
    {
        Events.OnLevelSetup+=LevelSetUp;
        Events.OnGameReset+=Reset; 
        Events.OnNextLevel+=NextLevel; 
    }

    void OnDisable()
    {
        Events.OnLevelSetup-=LevelSetUp; 
        Events.OnGameReset-=Reset;
        Events.OnNextLevel-=NextLevel; 
    }

    void Reset()
    {
        Destroy(C_lvl);
        C_camera.LookAt = null;
        C_camera.Follow = null;
    }

    public void NextLevel()
    {
        if(LevelNumber==(Alllevels.levels.Count-1)){
            LevelNumber=0;   
        }else{
            LevelNumber++;
        }
        Events.LevelSetup(LevelNumber);
    }

    void Start()
    {
        LevelNumber=DataManager.Instance.data.Current_Level;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
    }

    public void LevelSetUp(int _lvlnum){
        C_lvl= Instantiate(Alllevels.levels[_lvlnum]);
        levelGenerator=C_lvl.GetComponent<LevelGenerator>();
        C_Player=Instantiate(Player,levelGenerator.CurrentList[0].transform.position+new Vector3(0,0.9f,0),Quaternion.identity,C_lvl.transform);
        C_camera.Follow=C_Player.transform;
        C_camera.LookAt=C_Player.transform;
        if(_lvlnum>1){
            for (int i = 0; i < AiplayerCount; i++)
            {
                GameObject bot=Instantiate(Aiplayer,levelGenerator.CurrentList[i+1].transform.position+new Vector3(0,0.9f,0),Quaternion.identity,C_lvl.transform);
                C_Aiplayer.Add(bot);
            }
        }else{
            GameObject bot=Instantiate(Aiplayer,levelGenerator.CurrentList[1].transform.position+new Vector3(0,0.9f,0),Quaternion.identity,C_lvl.transform);
            C_Aiplayer.Add(bot);
        }
    }
}

