using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBot : MonoBehaviour
{
    public Animator CharacterAnimator;
    private bool _isEnableMovement,_isAutoRotateCharacter;
    protected bool isEnableMovement=>_isEnableMovement;
    private Vector3 _nextStagePosition,_characterPosition = Vector3.zero;
    public float SpeedHorizontal,HeightNormal,SpeedJump,SpeedGravity,AutoRotationSpeed;
    public bool isStartRace,isRaceFinished;
    private int _targetDir = -1; 
    private float _heightCurrent,_extraVerticalSpeed,_acceleration = 1; 
    private Quaternion _targetRotation; 
    private float _actualVerticalSpeed=> SpeedJump + _extraVerticalSpeed;
    private float _verticalVelocity;    
    protected bool isHorizontalMovement { get; set; }
    [SerializeField] private float _heightStop;
    protected bool isHeightStop=>transform.position.y <= _heightStop;
    Trampolines trampolines;
    
    private void OnEnable()
    {
        Events.OnGameStart+=StartGame;
    }

    private void OnDisable()
    {
        Events.OnGameStart-=StartGame;
    }

    private void Start()
    {
        _isEnableMovement = true;
        _nextStagePosition=transform.position;
    }


    public void StartGame()=>isStartRace=true;

    public void HorizontalMovement()
    {
        if (isEnableMovement)
        {
            _nextStagePosition.Set(_nextStagePosition.x,transform.position.y,_nextStagePosition.z);
            if (transform.position != _nextStagePosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _nextStagePosition,SpeedHorizontal * Time.deltaTime);
            }
        }
    }

    void Update()
    {
        VerticalMovement();
        AutoRotateCharacter();
        HorizontalMovement();
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
            _acceleration = Mathf.SmoothDamp(_acceleration,_targetDir, ref _verticalVelocity,_targetDir == 1 ?0  :1);
            CheckHeight();
        }
    }

     void CheckHeight()
    {
        if (isHeightStop){
            _targetDir = 0;
            ForceReset();
        }
    }

    public void ForceReset()
    {
        _isEnableMovement = false;
        _isAutoRotateCharacter = false;
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

    public void JumpAnimation()
    {
        int num=Random.Range(0,2);
        CharacterAnimator.SetTrigger(num==0?"Jump":"Jump2");
    }
     void JumpFunc(float height)
    {
        _targetDir = 1; 
        _heightCurrent = transform.position.y + height;
    }

    private void StartAutoRotation(Vector3 target)
    {
         target.Set(target.x, 0, target.z);
        _characterPosition.Set(transform.position.x,0,transform.position.z);
        _targetRotation = Quaternion.LookRotation(target - _characterPosition);
        if (transform.rotation != _targetRotation)
            _isAutoRotateCharacter = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BouncyStage"))
        {
            int BoatWeek=Random.Range(0,100);
            trampolines=other.GetComponent<Trampolines>();
            if(BoatWeek%3==0&&isStartRace){
                if(trampolines.ConnectedTrampuline.GetComponent<Trampolines>()!=null){
                    if(trampolines.ConnectedTrampuline.GetComponent<Trampolines>().MyType!=TrampolinesTypes.Breakable){
                        _nextStagePosition = trampolines.ConnectedTrampuline.position;
                    }else{
                        _nextStagePosition = trampolines.ConnectedTrampuline.GetComponent<Trampolines>().ConnectedTrampuline.position;
                    }
                }else{
                    _nextStagePosition = trampolines.ConnectedTrampuline.position;
                }
            }else{
                _nextStagePosition = trampolines.transform.position;
            }
            JumpFunc(HeightNormal);
            trampolines.BoatTrampolineAnimate();
            JumpAnimation();
            StartAutoRotation(trampolines.ConnectedTrampuline.position);
        }
        if (other.CompareTag("EndStage"))
        {
            _isEnableMovement = false; 
        }
    }

}
