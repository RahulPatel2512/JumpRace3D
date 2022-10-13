using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public enum TrampolinesTypes{
    Normal,
    Breakable,
    LongJump,
    Movebal
}

public class Trampolines : MonoBehaviour
{
    public TrampolinesTypes MyType;
    public int Myid;
    [SerializeField] private Transform Leftwing,Rightwing,MoveObj;
    [SerializeField] private float wingspeed=0.5f;
    [SerializeField] private Animation anim;
    [SerializeField] private TextMeshPro Numbers_txt;
    [SerializeField] private ParticleSystem Wave;
    [SerializeField] private GameObject Booster,BrokenTramp,NormalTramp;
    public float OffsetX;
    int _number;
    public BoxCollider boxCollider;
    public Transform ConnectedTrampuline;
    public int Number{
        get{
            return _number;
        }
        set{
            _number=value;
            Numbers_txt.text=_number.ToString();
        }
    }
    public void SetBooster(bool active) { Booster.SetActive(active); }
    private void Start()
    {
        boxCollider=transform.GetComponent<BoxCollider>();
        if(MyType==TrampolinesTypes.Normal||MyType==TrampolinesTypes.Breakable||MyType==TrampolinesTypes.Movebal){
            Leftwing.transform.DOLocalRotate(new Vector3(0,0,-30f),wingspeed).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
            Rightwing.transform.DOLocalRotate(new Vector3(0,0,30f),wingspeed).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
        }
        if(MyType==TrampolinesTypes.Movebal){
            float x=MoveObj.localPosition.x-OffsetX;
            MoveObj.localPosition=new Vector3(x,MoveObj.localPosition.y,MoveObj.localPosition.z);
            MoveObj.DOLocalMoveX(x+OffsetX,1).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
        }
    }

    public void TrampolineAnimate(){
        if(MyType==TrampolinesTypes.Normal||MyType==TrampolinesTypes.LongJump||MyType==TrampolinesTypes.Movebal){
            Wave.Play();
            anim.Play();
        }else{
            boxCollider.enabled=false;
            NormalTramp.SetActive(false);
            BrokenTramp.SetActive(true);
            Helper.Execute(this,()=>Destroy(BrokenTramp),1f);
        }
        
    }
    public void BoatTrampolineAnimate(){
        anim.Play();
    }

}
