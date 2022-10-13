using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Obstacles : MonoBehaviour
{
    public float Rotatespeed;
    public Transform RotateObj;

    private void Start()
    {
        RotateObj.DOLocalRotate(new Vector3(0,0,180f),Rotatespeed).SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental);
    }
}
