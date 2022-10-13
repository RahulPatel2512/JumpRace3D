using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class LevelCompleteScreen : MonoBehaviour
{
    public Button Next_btn;
    
    private void OnEnable() {
        Next_btn.onClick.AddListener(lvlComplete_btn);
        Events.OnLevelSetup+=BasicSetup;
        Events.OnGameFinish+=LvlCompleteScreen;
    }

    private void OnDisable()
    {
        Next_btn.onClick.RemoveAllListeners();
        Events.OnLevelSetup-=BasicSetup;
        Events.OnGameFinish-=LvlCompleteScreen;
    }

    void BasicSetup(int lvl){
    }

    public void LvlCompleteScreen(){
        UIController.Instance.HideThisScreen(ScreenType.GamePlayScreen);
        Helper.Execute(this,()=>{
            UIController.Instance.ShowThisScreen(ScreenType.LevelCompleteScreen);
            Next_btn.GetComponent<EasyTween>().OpenCloseObjectAnimationDefine(true);
        },0.1f);
    }
    public void lvlComplete_btn(){
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        Next_btn.GetComponent<EasyTween>().OpenCloseObjectAnimationDefine(false);
        Helper.Execute(this,()=>{
            UIController.Instance.HideThisScreen(ScreenType.LevelCompleteScreen);
            Events.LevelReset();
            Helper.Execute(this,()=>{
                Events.Nextlevel();
                UIController.Instance.ShowThisScreen(ScreenType.GamePlayScreen);
                GamePlayScreen.Instance.StartGame();
            },0.25f);
        },0.5f);
    }
}
