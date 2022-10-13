using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class GameOverScreen :  MonoBehaviour
{
    [SerializeField] Button Retrybtn;
    private void OnEnable() {   
        Events.OnGameOver+=GameOverfunc;
        Retrybtn.onClick.AddListener(RetrybtnFunc);
    }

    private void OnDisable()
    {
        Events.OnGameOver-=GameOverfunc;
        Retrybtn.onClick.RemoveAllListeners();
    }

    void GameOverfunc(){
        UIController.Instance.HideThisScreen(ScreenType.GamePlayScreen);
        Helper.Execute(this,()=>{
            UIController.Instance.ShowThisScreen(ScreenType.LevelFailScreen);
        },0.5f);
    }

    void RetrybtnFunc(){
        UIController.Instance.HideThisScreen(ScreenType.LevelFailScreen);
        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        Events.LevelReset();
        Helper.Execute(this,()=>{
            Events.LevelSetup(GameManager.Instance.LevelNumber);
            UIController.Instance.ShowThisScreen(ScreenType.GamePlayScreen);
            GamePlayScreen.Instance.StartGame();
        },0.3f);
    }
}
