using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GamePlayScreen : GenericS<GamePlayScreen>
{
    public GameObject Taptostart;
    public ParticleSystem good,excellent;
    public Image progresbar;
    public Text Lvl_txt,NextLvl_txt;
    int level;
    public int c_temp;
    float _progress;
    public float Progress{
        get{
            return _progress;
        }
        set{
            _progress=value;
            progresbar.fillAmount=_progress;
        }
    }

    public void MapProgress(int current_Temp,bool iscenter){
        if(c_temp!=current_Temp){
            Progress=Helper.Map(GameManager.Instance.levelGenerator.CurrentList.Count,0,0,1,current_Temp);
            if(iscenter){
                excellent.Play();
            }else{
                good.Play();
            }
            c_temp=current_Temp;
        }
    }

     void OnEnable()
    {
        Events.OnLevelSetup+=LevelSetUp; 
    }

    void OnDisable()
    {
        Events.OnLevelSetup-=LevelSetUp; 
    }
    public void LevelSetUp(int lvl){
        Progress=0;
        level=lvl;
    }

    public void StartGame(){
        StartCoroutine(GameStrart(Taptostart,()=>{
            Events.StartGame();
        }));
    }

    IEnumerator GameStrart(GameObject obj,Action action){
        yield return new WaitForSeconds(0.8f);
        obj.SetActive(true);
        while(true){
            if(Input.GetMouseButtonDown(0) && !IsPointerOverUIObject() && obj.activeInHierarchy){
                break;
            }
            yield return null;
        }
        action?.Invoke();
        obj.SetActive(false);
    }


    public static bool IsPointerOverUIObject () {
        PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
        eventDataCurrentPosition.position = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult> ();
        EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
