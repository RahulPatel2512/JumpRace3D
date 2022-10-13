using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<Trampolines> CurrentList;
    public Transform FinishPoint;
    public LineRenderer lineRenderer;

    [EasyButtons.Button("SetLine & TrampulinCountText")]
    public void SetlinePoints(){
        lineRenderer.positionCount=CurrentList.Count;
        for (int i = 0; i < CurrentList.Count; i++)
        {
            lineRenderer.SetPosition(i,CurrentList[i].transform.position);
            CurrentList[i].Number=CurrentList.Count-i;
            CurrentList[i].Myid=CurrentList.Count-i;
            if(i<CurrentList.Count-1){
                CurrentList[i].ConnectedTrampuline= CurrentList[i+1].transform;
            }else{
                CurrentList[i].ConnectedTrampuline=FinishPoint;
            }
        }
    }

    private void Start()
    {
        GamePlayScreen.Instance.c_temp=CurrentList.Count;
    }
}
