using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;

public class PlayerController : FloorDetector
{
    [SerializeField] private GameObject Basedetector,Ragdoll;
    public Animator CharacterAnimator;
    private int _targetDir = -1; 
    public float HeightLong,HeightNormal,HeightPerfect; 
    private Vector3 _characterPosition = Vector3.zero,_rotatePlayer;
    private Quaternion _targetRotation; 
    private bool _isAutoRotateCharacter,_isEnableMovement,isgamestart;
    private float _extraVerticalSpeed,_verticalVelocity,_heightCurrent,_acceleration = 1;
    public float RotationSpeed,AutoRotationSpeed,SpeedHorizontal,SpeedFastJump,SpeedJump,SpeedGravity;
    protected bool isEnableMovement { get { return _isEnableMovement; } }
    private float _actualVerticalSpeed =>SpeedJump + _extraVerticalSpeed;
    protected bool isHorizontalMovement { get; set; }
    protected bool isHeightStop=>transform.position.y <= _heightStop;
    [SerializeField]
    private float _heightStop,_touchMin;
    public List<GameObject> FireTrails;
    public GameObject SpleshParticle,crown;
    Trampolines trampolines;
    private float _xAxis;
    public float X_Axis =>_xAxis;
    private bool _isDrag =>Mathf.Abs(_posStart.x - _posEnd.x) >= _touchMin;
    private Vector2 _posStart,_posEnd;
    public List<Transform> MainPlayer_t,Ragdoll_t;
    BoxCollider box;

    void OnEnable()
    {
        Events.OnGameFinish+=Finish;
        Events.OnGameStart+=StartGame;
        Events.OnGameOver+=GameOver;
    }

    void OnDisable()
    {
        Events.OnGameFinish-=Finish;
        Events.OnGameStart-=StartGame;
        Events.OnGameOver-=GameOver;

    }

    private void Start()
    {
        box=transform.GetComponent<BoxCollider>();
        Basedetector.SetActive(true);
        _isEnableMovement = true;
        _acceleration = 1;
        _targetDir = -1;
    }

    public void StartGame()=>isgamestart=true;

    public void Finish(){
        crown.SetActive(true);
        Basedetector.SetActive(false);
        _isEnableMovement = false; 
    }

    void GameOver(){
        Basedetector.SetActive(false);
    }

    void Update()
    {
        MobileTouch();
        VerticalMovement();
        AutoRotateCharacter();
        HorizontalMovement();
        base.UpdateRaycast();
    }

    public void MobileTouch(){
        #if UNITY_ANDROID && !UNITY_EDITOR
            foreach(Touch touch in Input.touches)
                {
                    if(touch.phase == TouchPhase.Began ||touch.phase == TouchPhase.Stationary)
                    {
                        _posStart = touch.position;
                        _posEnd = touch.position;
                        _xAxis = 0;
                    }
                    if(touch.phase == TouchPhase.Moved)
                    {
                        _posEnd = touch.position;

                        if (_isDrag)
                        {
                            _xAxis = touch.deltaPosition.x;
                        }
                    }
                }

                if (Input.touches.Length == 0){ _xAxis = 0;}
        #endif

    }

    public void HorizontalMovement()
    {
        if (Input.GetMouseButton(0) && isEnableMovement&&isgamestart)
        {
            if (_isEnableMovement)
            {
                transform.Translate(Vector3.forward * SpeedHorizontal* 1* Time.deltaTime);
            }

            if (!isHorizontalMovement)
                isHorizontalMovement = true;

            RotatePlayer();
            StopAutoRotation();
        }
        else
        {
            if (isHorizontalMovement)
                isHorizontalMovement = false;
        }
    }

    private void RotatePlayer()
    {
        #if UNITY_EDITOR
                transform.Rotate(new Vector3(0,Input.GetAxis("Mouse X"),0) * Time.deltaTime * RotationSpeed * 10);
        #endif
        #if UNITY_ANDROID && !UNITY_EDITOR
                _rotatePlayer.Set(0,X_Axis,0);
                transform.Rotate(_rotatePlayer * Time.deltaTime * RotationSpeed);
        #endif
    }


    private void VerticalMovement()
    {
        if (_isEnableMovement)
        {
            transform.Translate(Vector3.up* (_targetDir == 1 ?_actualVerticalSpeed :_acceleration < 0 ?SpeedGravity :_actualVerticalSpeed)* _acceleration* Time.deltaTime);
            if (transform.position.y >= _heightCurrent)
            {
                _targetDir = -1; 
                _extraVerticalSpeed = 0;
            }
            _acceleration = Mathf.SmoothDamp(_acceleration,_targetDir, ref _verticalVelocity,_targetDir == 1 ?0 :1);
            if (isHeightStop){
                _targetDir = 0;
                ForceReset();
            }
        }
    }

    private void AutoRotateCharacter()
    {
        if (_isAutoRotateCharacter &&!isHorizontalMovement)
        {
            if (transform.rotation != _targetRotation)
                transform.rotation = Quaternion.RotateTowards(transform.rotation,_targetRotation,AutoRotationSpeed*Time.deltaTime);
            else _isAutoRotateCharacter = false;
        }
    }

    private void StartAutoRotation(Vector3 target)
    {
         target.Set(target.x, 0, target.z);
        _characterPosition.Set(transform.position.x,0,transform.position.z);
        _targetRotation = Quaternion.LookRotation(target - _characterPosition);
        if (transform.rotation != _targetRotation)
            _isAutoRotateCharacter = true;
    }

    private void StopAutoRotation()
    {
        if(_isAutoRotateCharacter)
            _isAutoRotateCharacter = false;
    }

    private void ApplyExtraJumpSpeed()
    {
        _extraVerticalSpeed = SpeedFastJump;
    }

    private void InstantFall()
    {
        _targetDir = -1; 
        _extraVerticalSpeed = 0;
        _acceleration = 0;
    }

    private void ForceReset()
    {
         BoostEffect(false);
        _isEnableMovement = false;
        _isAutoRotateCharacter = false;
    }

    public void BoostEffect(bool istrue){
        for (int i = 0; i < FireTrails.Count; i++)
        {
            FireTrails[i].SetActive(istrue);
        }
    }

    public void MoveUp(float _hight,bool _isBoost){
        _targetDir = 1; 
        _heightCurrent = transform.position.y + _hight;
        int num=Random.Range(0,2);
        CharacterAnimator.SetTrigger(num==0?"Jump":"Jump2");
        BoostEffect(_isBoost);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BouncyStage"))
        {
            trampolines=other.GetComponent<Trampolines>();
            GamePlayScreen.Instance.MapProgress(trampolines.Myid,false);
            MoveUp(HeightNormal,false);
            trampolines.SetBooster(false);
            StartAutoRotation(trampolines.ConnectedTrampuline.position);
            trampolines.TrampolineAnimate();
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
        }
        else if (other.CompareTag("Booster"))
        {
            trampolines=other.transform.parent.GetComponent<Trampolines>();
            GamePlayScreen.Instance.MapProgress(trampolines.Myid,true);
            MoveUp(HeightPerfect,true);
            other.gameObject.SetActive(false);
            StartAutoRotation(trampolines.ConnectedTrampuline.position);
        }
        else if (other.CompareTag("LongBouncyStage"))
        {
            trampolines=other.GetComponent<Trampolines>();
            MoveUp(HeightLong,false);
            ApplyExtraJumpSpeed(); 
            trampolines.TrampolineAnimate();
        }
        else if (other.CompareTag("StageBottom"))
        {
            InstantFall();
        }
        else if (other.CompareTag("Obstacle"))
        {
            box.enabled=false;
            ForceReset();
            CharacterAnimator.gameObject.SetActive(false);
            for (int i = 0; i <MainPlayer_t.Count; i++)
            {
                Ragdoll_t[i]=MainPlayer_t[i];
            }
            Ragdoll.SetActive(true);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            Events.GameOver();
        }
        else if (other.CompareTag("Floor"))
        {
            _isEnableMovement = false; 
            CharacterAnimator.gameObject.SetActive(false);
            FloorLine.gameObject.SetActive(false);
            Instantiate(SpleshParticle,new Vector3(transform.position.x,other.transform.position.y+0.5f,transform.position.z),Quaternion.identity);
            MMVibrationManager.Haptic(HapticTypes.LightImpact);
            Events.GameOver();
        }else if (other.CompareTag("EndStage"))
        {
            GamePlayScreen.Instance.Progress=1;
              MMVibrationManager.Haptic(HapticTypes.LightImpact);
           Events.GameFinish();
        }
    }
}
