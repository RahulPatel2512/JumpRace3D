using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SpleshScreen : MonoBehaviour
{
    public Image progressBar;
    public float loadingtime;
    public Text loading;
    float timer;

  IEnumerator Start(){
 	loading.DOText("Loading...",2f).SetLoops(-1,LoopType.Restart);
        yield return new WaitForSeconds(2f);//2f
        timer=0;
        while (timer < loadingtime)
        {
			progressBar.fillAmount = Mathf.Lerp(0,1, timer / loadingtime);
			timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
         Events.LevelSetup(DataManager.Instance.data.Current_Level);
        UIController.Instance.HideThisScreen(ScreenType.SpleshScreen);
        yield return new WaitForSeconds(0.5f);
        UIController.Instance.ShowThisScreen(ScreenType.GamePlayScreen);
        GamePlayScreen.Instance.StartGame();
    }

}
